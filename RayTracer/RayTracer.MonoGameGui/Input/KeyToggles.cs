using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace RayTracer.MonoGameGui.Input
{
    public sealed class KeyToggles
    {
        readonly List<(KeyboardLatch latch, DiscreteInput input)> _simpleToggles = new List<(KeyboardLatch latch, DiscreteInput input)>();

        public KeyToggles()
        {
            AddSimpleToggles(
                (Keys.OemOpenBrackets, DiscreteInput.DecreaseRenderFidelity),
                (Keys.OemCloseBrackets, DiscreteInput.IncreaseRenderFidelity)
            );
        }

        private void AddSimpleToggles(params (Keys key, DiscreteInput input)[] simpleToggles)
        {
            foreach (var simple in simpleToggles)
            {
                _simpleToggles.Add((new KeyboardLatch(simple.key), simple.input));
            }
        }

        public DiscreteInput Update(KeyboardState keyboardState)
        {
            foreach (var simpleToggle in _simpleToggles)
            {
                if (simpleToggle.latch.IsTriggered(keyboardState))
                {
                    return simpleToggle.input;
                }
            }

            return DiscreteInput.None;
        }
    }
}
