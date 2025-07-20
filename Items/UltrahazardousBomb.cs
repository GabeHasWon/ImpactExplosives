namespace ImpactExplosives.Items;

internal class UltrahazardousBomb : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Bomb);
        Item.shoot = ModContent.ProjectileType<UltrahazardousBombProj>();
        Item.damage = 250;
        Item.shootSpeed = 8;
        Item.rare = ItemRarityID.Green;
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<ImpactBomb>(3)
        .AddIngredient(ItemID.CursedFlame, 4)
        .Register();
}

internal class UltrahazardousBombProj : ModProjectile
{
    public override string Texture => base.Texture.Replace("Proj", "");

    public override void SetStaticDefaults() => ProjectileID.Sets.Explosive[Type] = true;

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.Bomb);
        Projectile.width += 2;
        Projectile.height += 2;
    }

    public override bool? CanHitNPC(NPC target) => Projectile.Hitbox.Intersects(target.Hitbox) && !target.friendly;

    public override void AI()
    {
        Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(DamageClass.Ranged).ApplyTo(250);

        if (Projectile.timeLeft == 2)
        {
            if (!Main.dedServ)
                ImpactExplosives.BombVFX(Projectile);

            for (int i = 0; i < 8; ++i)
            {
                Vector2 vel = Main.rand.NextVector2Circular(3, 3);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, vel.X, vel.Y);
            }

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

internal class UltrahazardousBombIchor : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Bomb);
        Item.shoot = ModContent.ProjectileType<UltrahazardousBombIchorProj>();
        Item.damage = 250;
        Item.shootSpeed = 8;
        Item.rare = ItemRarityID.Green;
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<ImpactBomb>(3)
        .AddIngredient(ItemID.CursedFlame, 4)
        .Register();
}

internal class UltrahazardousBombIchorProj : ModProjectile
{
    public override string Texture => base.Texture.Replace("Proj", "");

    public override void SetStaticDefaults() => ProjectileID.Sets.Explosive[Type] = true;

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.Bomb);
        Projectile.width += 2;
        Projectile.height += 2;
    }

    public override bool? CanHitNPC(NPC target) => Projectile.Hitbox.Intersects(target.Hitbox) && !target.friendly;

    public override void AI()
    {
        if (Projectile.timeLeft == 2)
        {
            if (!Main.dedServ)
                ImpactExplosives.BombVFX(Projectile);

            for (int i = 0; i < 8; ++i)
            {
                Vector2 vel = Main.rand.NextVector2Circular(3, 3);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, vel.X, vel.Y);
            }

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