using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;

namespace NoclipMe;

public static class PlayerExtensions
{
  public static void ToggleNoclip(this CBasePlayerPawn pawn)
  {
    if (pawn.MoveType == MoveType_t.MOVETYPE_NOCLIP)
    {
      pawn.MoveType = MoveType_t.MOVETYPE_WALK;
      Schema.SetSchemaValue(pawn.Handle, "CBaseEntity", "m_nActualMoveType", 2);
      Utilities.SetStateChanged(pawn, "CBaseEntity", "m_MoveType");
    }
    else
    {
      pawn.MoveType = MoveType_t.MOVETYPE_NOCLIP;
      Schema.SetSchemaValue(pawn.Handle, "CBaseEntity", "m_nActualMoveType", 8);
      Utilities.SetStateChanged(pawn, "CBaseEntity", "m_MoveType");
    }
  }
}

public class NoclipMe : BasePlugin
{
  public override string ModuleName => "NoclipMe Plugin";
  public override string ModuleVersion => "0.0.1";
  public override string ModuleAuthor => "injurka";

  public override void Load(bool hotReload)
  {
    base.Load(hotReload);
  }

  [ConsoleCommand("noclipme", "NoclipMe")]
  [ConsoleCommand("css_noclipme", "NoclipMe")]
  [ConsoleCommand("sm_noclip", "NoclipMe")]
  [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
  public void OnNoclipCommand(CCSPlayerController client, CommandInfo _)
  {
    var pawn = client.PlayerPawn.Value;
    if (pawn == null
      || !pawn.IsValid
      || !(client is { PawnIsAlive: true, IsHLTV: false, Connected: PlayerConnectedState.PlayerConnected })
      ) return;

    pawn.ToggleNoclip();
  }
}
