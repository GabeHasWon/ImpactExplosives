namespace ImpactExplosives.Items;

internal class ImpactBomb : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Bomb);
        Item.shoot = ModContent.ProjectileType<ImpactBombProj>();
        Item.damage = 100;
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<RedGel>()
        .AddIngredient(ItemID.Bomb)
        .Register();
}

internal class ImpactBombProj : ModProjectile
{
    public override string Texture => base.Texture.Replace("Proj", "");

    public override void SetStaticDefaults() => ProjectileID.Sets.Explosive[Type] = true;
    public override void SetDefaults() => Projectile.CloneDefaults(ProjectileID.Bomb);
    public override bool? CanHitNPC(NPC target) => Projectile.Hitbox.Intersects(target.Hitbox) && !target.friendly;

    public override void AI()
    {
        Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(DamageClass.Ranged).ApplyTo(100);

        if (Projectile.timeLeft == 2)
        {
            if (!Main.dedServ)
                ImpactExplosives.BombVFX(Projectile);

            ImpactExplosives.CircleExplosion(Projectile, ImpactExplosives.ExplosionType.Bomb);

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

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        if (Projectile.timeLeft > 3)
            Projectile.timeLeft = 3;
    }
}
