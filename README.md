# UIParticle-Service

Provides a service wrapper for easier usage based
on [ParticleEffectForUGUI](https://github.com/mob-sakai/ParticleEffectForUGUI.git).

## Installation & Setup

### 1. Install via Git Package

To install the latest version of **UAudio**, use the following Git URL in Unity's **Package Manager**:
`https://github.com/rustamkhonoff/ParticleEffectForUGUI-Service.git`

### 2. Install a Specific Version

UIParticle-Service also supports version tags. To install a specific version, append the tag to the URL:
`hhttps://github.com/rustamkhonoff/ParticleEffectForUGUI-Service.git#1.0.0`
Replace `1.0.0` with the desired version number.

### 3. Create UIParticlesEffectsConfiguration.asset anywhere in Resources folder:

`Services/UI Particles/Create UIParticlesEffectsConfiguration`

### 4. Fill in all fields in the configuration `UIParticlesEffectsConfiguration.asset` file

> You can use default materials and canvas, they are located inside Resources folder in Package location

### 5. Manual service creation and usage

````csharp
    private UIParticlesEffectsConfiguration _particleConfiguration;
    ...
    UIParticleService = new DefaultUIParticleService(_particleConfiguration);    
````

````csharp
    UIParticleService.Attract(...);
    //or
    UIParticlesGlobal.Instance.Attract(...);
````

### 5.1 Setup service using Zenject

Add ZENJECT in ScriptingDefineSymbols in Player/Other Settings
![img.png](Documentation%2Fimg.png)
And in ProjectContext

````csharp
    //If UIParticlesEffectsConfiguration asset located in root Resources folder
    Container.AddUIParticlesService();
    //or 
    ontainer.AddUIParticlesService(a=> a.ConfigurationPath = "YOUR PATH");
````

> [NOTE] All particles are created inside a single Canvas Root. Make sure the Root Canvas prefab's order layer is higher
> than your default canvases order layer.

# Dependecies

```
  "dependencies": {
    "com.coffee.ui-particle": "4.9.0" (https://github.com/mob-sakai/ParticleEffectForUGUI.git)
  }
```

# Service API Reference

```csharp
    public interface IUIParticleService
    {
        void Attract(
            UIParticleConfiguration configuration,
            Action<Coffee.UIExtensions.UIParticle> configureUIParticle = null,
            Action<ParticleSystem> configureParticle = null,
            Action<UIParticleAttractor> configureAttractor = null
            );

        void ClearAll();
    }
```

# Usage sample

```csharp
     PositionInfo vectorPos = new(Vector3.zero);
     //same as
     PositionInfo vectorPosImplicit = Vector3.zero;

     PositionInfo rectPos = new(m_anyRectTransform);
     //same as
     PositionInfo rectPosImplicit = m_anyRectTransform;

     PositionInfo transformPos = new(m_anyTransform);
     //same as
     PositionInfo transformPosImplicit = m_anyTransform;

     //where sprite is source of texture, 10 is particles amount and vectorPos is particles destination position
     UIParticleConfiguration.Builder builder = new(m_sprite, 10, vectorPos);

     builder.WithAmount(20); //Change particles amount
     builder.WithTexture(m_otherTexture); //Change texture
     builder.WithEmitDelay(0.1f); //Set delay between each particle emit
     builder.WithStartPosition(Input.mousePosition); //Set particles spawn point to cursor position
     builder.WithTargetPosition(new PositionInfo(new Vector3(10, 10, 10)));
     builder.WithFirstAttractCallback(() => Debug.Log("First particle attracted"));
     builder.WithAttractAction(() => Debug.Log("Particle attracted"));
     builder.WithEndCallback(() => Debug.Log("Particles stops emitting"));
     builder.WithEmitInfo(new EmittingInfo(delay: 0.1f, amount: 20));

     //Initiate attract
     m_particleService.Attract(builder.Build());
    
     //Destroy all existing particles
     m_particleService.ClearAll();
```

## Position Info

**Spacetype means where the target position is, in the _UI_ or in the _World_**

> Use SpaceType.UI when the particle spawn/target position is in the UI, inside (the canvas)
> Use SpaceType.World when the particle spawn/target position is in the World space

```csharp
    public PositionInfo(Vector3 vector3, SpaceType space = SpaceType.UI)
    public PositionInfo(Transform transform, SpaceType space = SpaceType.World, bool updatePositionOnUpdate = true)
    public PositionInfo(RectTransform rectTransform, SpaceType space = SpaceType.UI, bool updatePositionOnUpdate = true)
    public PositionInfo(Func<Vector3> positionFunc, SpaceType space = SpaceType.UI, bool updatePositionOnUpdate = true)
    //and
    public static PositionInfo ScreenCenter => new(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
```
