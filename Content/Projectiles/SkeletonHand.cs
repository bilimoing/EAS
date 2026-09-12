using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Projectiles;

public class SkeletonHand : ModProjectile
{
    private const int AttackInterval = 48;
    private const int AttackStart = 28;
    private const int AttackEnd = 38;
    private const float OrbitRadius = 76f;
    private const float OrbitVerticalRadius = 48f;
    private const float OrbitSpeed = 0.035f;

    private ref float HandIndex => ref Projectile.ai[0];
    private ref float HandCount => ref Projectile.ai[1];
    private ref float TargetIndex => ref Projectile.ai[2];

    private float OrbitTimer
    {
        get => Projectile.localAI[0];
        set => Projectile.localAI[0] = value;
    }

    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 3;
        ProjectileID.Sets.DrawScreenCheckFluff[Type] = 400;
    }

    public override void SetDefaults()
    {
        Projectile.width = 36;
        Projectile.height = 36;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 2;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.netImportant = true;
    }

    public override bool? CanDamage()
    {
        int attackFrame = (int)(OrbitTimer + HandIndex * (AttackInterval / 2f)) % AttackInterval;
        return attackFrame is >= AttackStart and <= AttackEnd;
    }

    public override void AI()
    {
        Player player = FindOwnerPlayer();
        if (player is not { active: true } || player.dead || !player.GetModPlayer<Common.Players.AccPlayer>().DeathBone)
        {
            Projectile.Kill();
            return;
        }

        Projectile.timeLeft = 2;
        Projectile.damage = Main.dayTime ? 56 : 28;
        OrbitTimer++;

        NPC target = GetTarget(player);
        OrbitAroundPlayer(player, target);
        Projectile.frame = (int)(Main.GameUpdateCount / 8 % 3);
    }

    private Player FindOwnerPlayer()
    {
        if (Main.player == null)
        {
            return null;
        }

        return Main.player.FirstOrDefault(candidate => candidate != null && candidate.whoAmI == Projectile.owner);
    }

    private void OrbitAroundPlayer(Player player, NPC target)
    {
        int count = Math.Max(1, (int)HandCount);
        float angle = OrbitTimer * OrbitSpeed + MathHelper.TwoPi * HandIndex / count;

        float horizontalDepth = MathF.Cos(angle);
        float verticalDepth = MathF.Sin(angle);
        Vector2 orbitPosition = player.Center + new Vector2(horizontalDepth * OrbitRadius, verticalDepth * OrbitVerticalRadius);
        int attackFrame = (int)(OrbitTimer + HandIndex * (AttackInterval / 2f)) % AttackInterval;
        bool attacking = attackFrame is >= AttackStart and <= AttackEnd;

        if (attacking && target != null)
        {
            TargetIndex = target.whoAmI;
            Vector2 toTarget = target.Center - Projectile.Center;
            Vector2 attackDirection = toTarget.SafeNormalize(Vector2.UnitX * (target.Center.X >= player.Center.X ? 1f : -1f));
            Vector2 attackPosition = target.Center - attackDirection * 12f;
            MoveTowards(attackPosition, 34f);
            Projectile.rotation = attackDirection.ToRotation();
            Projectile.spriteDirection = attackDirection.X >= 0f ? 1 : -1;
        }
        else
        {
            TargetIndex = -1f;
            MoveTowards(orbitPosition, 14f);
            Projectile.rotation = angle + MathHelper.PiOver2;
            Projectile.spriteDirection = horizontalDepth >= 0f ? 1 : -1;
        }
        Projectile.scale = MathHelper.Lerp(0.72f, 1.18f, (verticalDepth + 1f) * 0.5f);
    }

    private NPC GetTarget(Player player)
    {
        int savedTargetIndex = (int)TargetIndex;
        if (savedTargetIndex >= 0 && savedTargetIndex < Main.npc.Length && IsValidTarget(Main.npc[savedTargetIndex]) && Vector2.DistanceSquared(player.Center, Main.npc[savedTargetIndex].Center) <= 1600f * 1600f)
        {
            return Main.npc[savedTargetIndex];
        }

        NPC closest = null;
        float closestDistance = float.MaxValue;
        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (IsValidTarget(npc) && !(Vector2.DistanceSquared(player.Center, npc.Center) > 1600f * 1600f))
            {
                float distance = Vector2.DistanceSquared(player.Center, npc.Center);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = npc;
                }
            }
        }

        return closest;
    }

    private static bool IsValidTarget(NPC npc)
    {
        return npc is { active: true, friendly: false, townNPC: false } && npc.CanBeChasedBy() && npc.lifeMax > 0;
    }

    private void MoveTowards(Vector2 destination, float maxSpeed)
    {
        Vector2 difference = destination - Projectile.Center;
        if (difference.LengthSquared() > maxSpeed * maxSpeed)
        {
            difference = Vector2.Normalize(difference) * maxSpeed;
        }

        Projectile.velocity = Vector2.Lerp(Projectile.velocity, difference, 0.22f);
        Projectile.Center += Projectile.velocity;
    }

    public override bool PreDraw(Player player, ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        int frameHeight = texture.Height / Main.projFrames[Type];
        Rectangle sourceRectangle = new(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
        SpriteEffects effects = Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Vector2 origin = sourceRectangle.Size() / 2f;

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceRectangle, lightColor, Projectile.rotation, origin, Projectile.scale, effects);
        return false;
    }
}
