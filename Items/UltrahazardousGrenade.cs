using Terraria.Audio;

namespace ImpactExplosives.Items;

internal class UltrahazardousGrenade : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Grenade);
        Item.shoot = ModContent.ProjectileType<UltrahazardousGrenadeProj>();
        Item.damage = 180;
        Item.shootSpeed = 10;
        Item.rare = ItemRarityID.Green;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => knockback = 15;

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<ImpactGrenade>(3)
        .AddIngredient(ItemID.CursedFlame, 2)
        .Register();
}

internal class UltrahazardousGrenadeProj : ModProjectile
{
    public override string Texture => base.Texture.Replace("Proj", "");

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.Grenade);
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
    }

    public override bool? CanHitNPC(NPC target) => Projectile.Hitbox.Intersects(target.Hitbox) && !target.friendly;

    public override void AI()
    {
        Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(DamageClass.Ranged).ApplyTo(180);

        if (Projectile.timeLeft == 2)
        {
            if (!Main.dedServ)
            {
                ImpactExplosives.BombVFX(Projectile);
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

                for (int i = 0; i < 4; ++i)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(3, 3);
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, vel.X, vel.Y);
                }
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
}

internal class UltrahazardousGrenadeIchor : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Grenade);
        Item.shoot = ModContent.ProjectileType<UltrahazardousGrenadeIchorProj>();
        Item.damage = 180;
        Item.shootSpeed = 10;
        Item.rare = ItemRarityID.Green;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => knockback = 15;

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<ImpactGrenade>(3)
        .AddIngredient(ItemID.Ichor, 2)
        .Register();
}

internal class UltrahazardousGrenadeIchorProj : ModProjectile
{
    public override string Texture => base.Texture.Replace("Proj", "");

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.Grenade);
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
    }

    public override bool? CanHitNPC(NPC target) => Projectile.Hitbox.Intersects(target.Hitbox) && !target.friendly;

    public override void AI()
    {
        Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(DamageClass.Ranged).ApplyTo(180);

        if (Projectile.timeLeft == 2)
        {
            if (!Main.dedServ)
            {
                ImpactExplosives.BombVFX(Projectile);
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

                for (int i = 0; i < 4; ++i)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(3, 3);
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, vel.X, vel.Y);
                }
            }

            Projectile.Resize(128, 128);
            Projectile.hide = true;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.timeLeft = 3;
        return true;
    }

    public override bool CanHitPvp(Player target) => Projectile.timeLeft is 2 or > 3;
    public override bool CanHitPlayer(Player target) => Projectile.timeLeft is 2 or > 3;

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        if (Projectile.timeLeft > 3)
            Projectile.timeLeft = 3;
    }
}
