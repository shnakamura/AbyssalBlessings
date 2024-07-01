using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AbyssalBlessings.Common.Graphics.Particles;

public abstract class Particle
{
    public float Alpha = 1f;

    public Color Color = Color.White;
    public Vector2 Position;

    public float Rotation;

    public float Scale = 1f;

    public Vector2 Velocity;

    public virtual string Texture => GetType().FullName.Replace('.', '/');

    public void Kill() {
        ParticleManager.Kill(this);
    }
    
    public virtual void OnCreate() { }
    
    public virtual void OnKill() { }

    public virtual void OnUpdate() { }

    public virtual bool PreDraw() {
        return true;
    }
    
    public virtual void PostDraw() { }
}
