namespace VolunterProg.Application.Voluunter.UpdateModule;

public record UpdateVoluunterRequest(
    Guid Id, 
    VoluunterDto VoluunterDto);