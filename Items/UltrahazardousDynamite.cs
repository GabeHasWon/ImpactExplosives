namespace ImpactExplosives.Items;

internal class UltrahazardousDynamite : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Dynamite);
        Item.shoot = ModContent.ProjectileType<UltrahazardousDynamiteProj>();
        Item.damage = 350;
        Item.shootSpeed = 0;
        Item.rare = ItemRarityID.Green;
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<ImpactDynamite>(3)
        .AddIngredient(ItemID.CursedFlame, 7)
        .Register();
}

internal class UltrahazardousDynamiteProj : ModProjectile
{
    public override string Texture => base.Texture.Replace("Proj", "");

    public override void SetStaticDefaults() => ProjectileID.Sets.Explosive[Type] = true;

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.Dynamite);
        Projectile.height += 2;
    }

    public override bool? CanHitNPC(NPC target) => Projectile.Hitbox.Intersects(target.Hitbox) && !target.friendly;

    public override void AI()
    {
        Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(DamageClass.Ranged).ApplyTo(350);

        if (Projectile.timeLeft == 2)
        {
            if (!Main.dedServ)
                ImpactExplosives.BombVFX(Projectile);

            for (int i = 0; i < 8; ++i)
            {
                Vector2 vel = Main.rand.NextVector2Circular(3, 3);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, vel.X, vel.Y);
            }

            ImpactExplosives.CircleExplosion(Projectile, ImpactExplosives.ExplosionType.Dynamite, 2);

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

internal class UltrahazardousDynamiteIchor : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Dynamite);
        Item.shoot = ModContent.ProjectileType<UltrahazardousDynamiteIchorProj>();
        Item.damage = 350;
        Item.shootSpeed = 0;
        Item.rare = ItemRarityID.Green;
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient<ImpactDynamite>(3)
        .AddIngredient(ItemID.Ichor, 7)
        .Register();
}

internal class UltrahazardousDynamiteIchorProj : ModProjectile
{
    public override string Texture => base.Texture.Replace("Proj", "");

    public override void SetStaticDefaults() => ProjectileID.Sets.Explosive[Type] = true;

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.Dynamite);
        Projectile.height += 2;
    }

    public override bool? CanHitNPC(NPC target) => Projectile.Hitbox.Intersects(target.Hitbox) && !target.friendly;

    public override void AI()
    {
        Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(DamageClass.Ranged).ApplyTo(350);

        if (Projectile.timeLeft == 2)
        {
            if (!Main.dedServ)
                ImpactExplosives.BombVFX(Projectile);

            for (int i = 0; i < 8; ++i)
            {
                Vector2 vel = Main.rand.NextVector2Circular(3, 3);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, vel.X, vel.Y);
            }

            ImpactExplosives.CircleExplosion(Projectile, ImpactExplosives.ExplosionType.Dynamite, 2);

            Projectile.PrepareBombToBlow();
            Projectile.Resize(200, 200);
            Projectile.hide = true;
        }
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) => modifiers.FinalDamage += 0.5f;

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