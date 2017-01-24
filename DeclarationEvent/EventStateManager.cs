using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Walterlv.Events
{
    internal sealed class EventStateManager
    {
        private readonly Dictionary<DeclarationChain, int> _chainStepDictionary;

        internal EventStateManager(IEnumerable<DeclarationChain> nodes)
        {
            _chainStepDictionary = nodes.ToDictionary(x => x, x => 0);
        }

        internal void Step<T>(T node) where T : DeclarationChainNode
        {
            var exceptions = new List<Exception>();

            var toRemove = new List<DeclarationChain>();
            foreach (var pair in _chainStepDictionary)
            {
                var chain = pair.Key;
                var step = pair.Value;
                var pass = chain[step].CanPassStep(node);

                switch (pass)
                {
                    case StateStep.Ignore:
                        break;
                    case StateStep.Pass:
                        step++;
                        if (step == chain.Count)
                        {
                            try
                            {
                                Confirmed?.Invoke(chain);
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(ex);
                            }
                        }
                        _chainStepDictionary[pair.Key] = step;
                        break;
                    case StateStep.Abort:
                        toRemove.Add(pair.Key);
                        _chainStepDictionary.Remove(pair.Key);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            toRemove.ForEach(x => _chainStepDictionary.Remove(x));

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    ExceptionDispatchInfo.Capture(exceptions.First()).Throw();
                }
                else
                {
                    throw new AggregateException(exceptions);
                }
            }
        }

        internal Action<DeclarationChain> Confirmed { get; set; }
    }
}
