using System.Collections.Generic;
using AbyssalBlessings.Common.Graphics.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Graphics.Particles;

[Autoload(Side = ModSide.Client)]
public sealed class ParticleManager : ModSystem
{
    public static List<Particle> Particles { get; } = new();
    
    public static void Create<T>(T particle) where T : Particle {
        particle.OnCreate();
        
        Particles.Add(particle);
    }

    public static void Kill<T>(T particle) where T : Particle {
        particle.OnKill();
        
        Particles.Remove(particle);
    }

    public override void PreUpdateWorld() {
        UpdateParticles();
        RenderParticles();
    }

    private static void UpdateParticles() {
        for (var i = 0; i < Particles.Count; i++) {
            var particle = Particles[i];

            particle.Position += particle.Velocity;

            particle.OnUpdate();
        }
    }

    private static void RenderParticles() {
        for (var i = 0; i < Particles.Count; i++) {
            var particle = Particles[i];

            var texture = ModContent.Request<Texture2D>(particle.Texture).Value;

            PixellatedRenderer.Queue(
                () => {
                    var result = particle.PreDraw();
                    
                    if (result) {
                        Main.EntitySpriteDraw(
                            texture, 
                            particle.Position - Main.screenPosition / 2f, 
                            null, 
                            particle.Color * particle.Alpha, 
                            particle.Rotation,
                            texture.Size() / 2f,
                            particle.Scale, 
                            SpriteEffects.None
                        );
                    }
                    
                    particle.PostDraw();
                }
            );
        }
    }
}
