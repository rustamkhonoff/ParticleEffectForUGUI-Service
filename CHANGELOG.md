# Changelog

## [1.1.0]
### New
1. Removed unnesesary parameters for Particle Configuration
2. Now ParticleSystem, UIParticle or UIParticleAttarctor can be configured directly
```csharp
 void Attract(UIParticleAttractConfiguration configuration,
            Action<UIParticle> configureUIParticle = null, 
            Action<ParticleSystem> configureParticle = null,
            Action<UIParticleAttractor> configureAttractor = null);
```
3. Added ClearAll to destroy all created UIParticles under parent Canvas
4. Removed unnesesary reflection in _**UIParticleAttractorView.cs**_
5. Creating Configuration moved to Builder style _**UIParticleAttractConfiguration.Builder**_
6. Any position like Vector3 or Transform (StartPosition,TargetPosition) changed to _**PositionInfo**_

