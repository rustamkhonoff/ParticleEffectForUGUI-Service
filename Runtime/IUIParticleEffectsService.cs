#if COFFEE_PARTICLES
using UGUIParticleEffect.Builder;

namespace UGUIParticleEffect
{
    public interface IUIParticleEffectsService
    {
        void Attract(UIParticleAttractConfiguration configuration);
    }
}

#endif