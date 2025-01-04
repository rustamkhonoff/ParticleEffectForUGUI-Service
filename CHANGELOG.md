# Changelog

## [1.1.0]
### Changes
- Namespace changes
- Updated depreciated UIParticle API 
- Renamed .asmdef

## [1.1.0]
### Changes
1. Removed unnesesary parameters for Particle Configuration
2. Removed unnesesary reflection in _**UIParticleAttractorView.cs**_
### New
1. Now ParticleSystem, UIParticle or UIParticleAttarctor can be configured directly
```csharp
 void Attract(UIParticleAttractConfiguration configuration,
            Action<UIParticle> configureUIParticle = null, 
            Action<ParticleSystem> configureParticle = null,
            Action<UIParticleAttractor> configureAttractor = null);
```
2. Added ClearAll to destroy all created UIParticles under parent Canvas

3. Creating Configuration moved to Builder style _**UIParticleAttractConfiguration.Builder**_
4. Any position like Vector3 or Transform (StartPosition,TargetPosition) changed to _**PositionInfo**_

