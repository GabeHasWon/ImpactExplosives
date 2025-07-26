using Terraria.Audio;
using Terraria.ID;

namespace ImpactExplosives.Items;

internal class ImpactGrenade : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Grenade);
        Item.shoot = ModContent.ProjectileType<ImpactGrenadeProj>();
        Item.damage = 75;
    }

    public override void AddRecipes() => CreateRecipe(5)
        .AddIngredient<RedGel>()
        .AddIngredient(ItemID.Grenade, 5)
        .Register();
}

internal class ImpactGrenadeProj : ModProjectile
{
    public override string Texture => base.Texture.Replace("Proj", "");

    public override void SetStaticDefaults() => ProjectileID.Sets.Explosive[Type] = true;

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.Grenade);
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
    }

    public override bool? CanHitNPC(NPC target) => Projectile.Hitbox.Intersects(target.Hitbox) && !target.friendly;

    public override void AI()
    {
        Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(DamageClass.Ranged).ApplyTo(75);

        if (Projectile.timeLeft == 2)
        {
            if (!Main.dedServ)
            {
                ImpactExplosives.BombVFX(Projectile);
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            }

            Projectile.PrepareBombToBlow();
            Projectile.Resize(128, 128);
            Projectile.hide = true;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.timeLeft = 3;
        return true;
    }

    // Safety net for multihits on players
    public override bool CanHitPvp(Player target) => Projectile.timeLeft is 2 or > 3;
    public override bool CanHitPlayer(Player target) => Projectile.timeLeft is 2 or > 3;

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        if (Projectile.timeLeft > 3)
            Projectile.timeLeft = 3;
    }
}
