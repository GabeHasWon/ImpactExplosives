global using Microsoft.Xna.Framework;
global using Terraria;
global using Terraria.ID;
global using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ImpactExplosives;

public class ImpactExplosives : Mod
{
    public enum ExplosionType
    {
        Bomb,
        Dynamite
    }

    public static void CircleExplosion(Projectile proj, ExplosionType type, int sizeAdd = 0)
    {
        int size = 3;

        if (type == ExplosionType.Bomb)
            size = 4;

        if (type == ExplosionType.Dynamite)
            size = 7;

        size += sizeAdd;
        Vector2 pos = proj.position;

        int width = size;
        int height = size;
        int minX = (int)(pos.X / 16f - width);
        int maxX = (int)(pos.X / 16f + width);
        int minY = (int)(pos.Y / 16f - height);
        int maxY = (int)(pos.Y / 16f + height);

        if (minX < 0)
            minX = 0;
        if (maxX > Main.maxTilesX)
            maxX = Main.maxTilesX;
        if (minY < 0)
            minY = 0;
        if (maxY > Main.maxTilesY)
            maxY = Main.maxTilesY;

        bool wallSplode2 = proj.ShouldWallExplode(pos, size, minX, maxX, minY, maxY);
        proj.ExplodeTiles(pos, size, minX, maxX, minY, maxY, wallSplode2);
    }

    public static void BombVFX(Projectile projectile)
    {
        SoundEngine.PlaySound(in SoundID.Item14, projectile.position);
        int dustId = DustID.Torch;

        for (int i = 0; i < 20; i++)
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
            Dust newDust = Main.dust[dust];
            newDust.velocity *= 1.4f;
        }

        for (int i = 0; i < 10; i++)
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustId, 0f, 0f, 100, default(Color), 2.5f);
            Dust newDust = Main.dust[dust];
            newDust.noGravity = true;
            newDust.velocity *= 5f;

            dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustId, 0f, 0f, 100, default(Color), 1.5f);
            newDust = Main.dust[dust];
            newDust.velocity *= 3f;
        }

        IEntitySource src = projectile.GetSource_Death();
        int slot = Gore.NewGore(src, projectile.position, default, Main.rand.Next(61, 64));
        Gore gore = Main.gore[slot];
        gore.velocity *= 0.4f;
        gore.velocity.X += 1f;
        gore.velocity.Y += 1f;

        slot = Gore.NewGore(src, projectile.position, default, Main.rand.Next(61, 64));
        gore = Main.gore[slot];
        gore.velocity *= 0.4f;
        gore.velocity.X -= 1f;
        gore.velocity.Y += 1f;

        slot = Gore.NewGore(src, projectile.position, default, Main.rand.Next(61, 64));
        gore = Main.gore[slot];
        gore.velocity *= 0.4f;
        gore.velocity.X += 1f;
        gore.velocity.Y -= 1f;

        slot = Gore.NewGore(src, projectile.position, default, Main.rand.Next(61, 64));
        gore = Main.gore[slot];
        gore.velocity *= 0.4f;
        gore.velocity.X -= 1f;
        gore.velocity.Y -= 1f;
    }

    public static void DynamiteVFX(Projectile projectile, bool adjust, bool unadjust)
    {
        SoundEngine.PlaySound(in SoundID.Item14, projectile.position);

        if (adjust)
        {
            projectile.Resize(200, 200);
            projectile.position += projectile.Size / 2f;
            projectile.Size = new Vector2(200);
            projectile.position -= projectile.Size / 2f;
        }

        for (int num983 = 0; num983 < 50; num983++)
        {
            int num984 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
            Dust dust83 = Main.dust[num984];
            Dust dust334 = dust83;
            dust334.velocity *= 1.4f;
        }

        for (int num985 = 0; num985 < 80; num985++)
        {
            int num986 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
            Main.dust[num986].noGravity = true;
            Dust dust84 = Main.dust[num986];
            Dust dust334 = dust84;
            dust334.velocity *= 5f;
            num986 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
            dust84 = Main.dust[num986];
            dust334 = dust84;
            dust334.velocity *= 3f;
        }

        for (int num988 = 0; num988 < 2; num988++)
        {
            int num989 = Gore.NewGore(projectile.GetSource_Death(), projectile.Center - new Vector2(24), default(Vector2), Main.rand.Next(61, 64));
            Main.gore[num989].scale = 1.5f;
            Main.gore[num989].velocity.X += 1.5f;
            Main.gore[num989].velocity.Y += 1.5f;
            num989 = Gore.NewGore(projectile.GetSource_Death(), projectile.Center - new Vector2(24), default(Vector2), Main.rand.Next(61, 64));
            Main.gore[num989].scale = 1.5f;
            Main.gore[num989].velocity.X -= 1.5f;
            Main.gore[num989].velocity.Y += 1.5f;
            num989 = Gore.NewGore(projectile.GetSource_Death(), projectile.Center - new Vector2(24), default(Vector2), Main.rand.Next(61, 64));
            Main.gore[num989].scale = 1.5f;
            Main.gore[num989].velocity.X += 1.5f;
            Main.gore[num989].velocity.Y -= 1.5f;
            num989 = Gore.NewGore(projectile.GetSource_Death(), projectile.Center - new Vector2(24), default(Vector2), Main.rand.Next(61, 64));
            Main.gore[num989].scale = 1.5f;
            Main.gore[num989].velocity.X -= 1.5f;
            Main.gore[num989].velocity.Y -= 1.5f;
        }

        if (unadjust)
            projectile.Resize(10, 10);
    }
}
