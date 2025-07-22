namespace ImpactExplosives.Items;

internal class ImpactDynamite : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Dynamite);
        Item.shoot = ModContent.ProjectileType<ImpactDynamiteProj>();
        Item.damage = 250;
        Item.shootSpeed = 1;
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<RedGel>()
        .AddIngredient(ItemID.Dynamite)
        .Register();
}

internal class ImpactDynamiteProj : ModProjectile
{
    public override string Texture => base.Texture.Replace("Proj", "");

    public override void SetStaticDefaults() => ProjectileID.Sets.Explosive[Type] = true;
    public override void SetDefaults() => Projectile.CloneDefaults(ProjectileID.Dynamite);
    public override bool? CanHitNPC(NPC target) => Projectile.Hitbox.Intersects(target.Hitbox) && !target.friendly;

    public override void AI()
    {
        Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(DamageClass.Ranged).ApplyTo(250);

        if (Projectile.timeLeft == 2)
        {
            if (!Main.dedServ)
                ImpactExplosives.BombVFX(Projectile);

            ImpactExplosives.CircleExplosion(Projectile, ImpactExplosives.ExplosionType.Dynamite);

            Projectile.PrepareBombToBlow();
            Projectile.Resize(200, 200);
            Projectile.hide = true;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.timeLeft = 3;
        return true;
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) => modifiers.FinalDamage += 0.55f;

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        if (Projectile.timeLeft > 3)
            Projectile.timeLeft = 3;
    }
}
