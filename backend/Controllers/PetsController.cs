using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Pets;
using PetHaven.Api.Models;

namespace PetHaven.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class PetsController : BaseApiController
{
    private readonly AppDbContext _context;

    public PetsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all pet profiles belonging to the current user
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<PetProfileDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyPets()
    {
        var userId = GetCurrentUserId();
        var pets = await _context.PetProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Include(p => p.PetType)
            .OrderBy(p => p.Name)
            .Select(p => MapPetToDto(p))
            .ToListAsync();

        return Ok(ApiResponse<List<PetProfileDto>>.Ok(pets));
    }

    /// <summary>
    /// Retrieve single pet profile by ID
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<PetProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PetProfileDto>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<PetProfileDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPetById(ulong id)
    {
        var pet = await _context.PetProfiles
            .AsNoTracking()
            .Include(p => p.PetType)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pet == null)
        {
            return NotFound(ApiResponse<PetProfileDto>.Fail("Pet profile not found.", 404));
        }

        var userId = GetCurrentUserId();
        if (pet.UserId != userId && !IsAdminOrStaff())
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<PetProfileDto>.Fail("You cannot view this pet profile.", 403));
        }

        return Ok(ApiResponse<PetProfileDto>.Ok(MapPetToDto(pet)));
    }

    /// <summary>
    /// Create a new "My Pet" profile
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PetProfileDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<PetProfileDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePet([FromBody] CreatePetProfileDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<PetProfileDto>.Fail("Invalid pet profile information.", 400));
        }

        var petType = await _context.PetTypes.FindAsync(dto.PetTypeId);
        if (petType == null)
        {
            return BadRequest(ApiResponse<PetProfileDto>.Fail("Invalid pet type ID.", 400));
        }

        var userId = GetCurrentUserId();
        var pet = new PetProfile
        {
            UserId = userId,
            PetTypeId = dto.PetTypeId,
            Name = dto.Name.Trim(),
            Breed = dto.Breed?.Trim(),
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            Weight = dto.Weight,
            ProfileImageUrl = dto.ProfileImageUrl,
            DietaryPreferences = dto.DietaryPreferences,
            AllergiesOrMedicalNotes = dto.AllergiesOrMedicalNotes,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.PetProfiles.Add(pet);
        await _context.SaveChangesAsync();

        pet.PetType = petType;
        return StatusCode(StatusCodes.Status201Created, ApiResponse<PetProfileDto>.Created(MapPetToDto(pet), "Pet profile created."));
    }

    /// <summary>
    /// Update an existing pet profile
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<PetProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PetProfileDto>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<PetProfileDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePet(ulong id, [FromBody] UpdatePetProfileDto dto)
    {
        var pet = await _context.PetProfiles.Include(p => p.PetType).FirstOrDefaultAsync(p => p.Id == id);
        if (pet == null)
        {
            return NotFound(ApiResponse<PetProfileDto>.Fail("Pet profile not found.", 404));
        }

        var userId = GetCurrentUserId();
        if (pet.UserId != userId && !IsAdminOrStaff())
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<PetProfileDto>.Fail("You cannot update this pet profile.", 403));
        }

        pet.PetTypeId = dto.PetTypeId;
        pet.Name = dto.Name.Trim();
        pet.Breed = dto.Breed?.Trim();
        pet.Gender = dto.Gender;
        pet.DateOfBirth = dto.DateOfBirth;
        pet.Weight = dto.Weight;
        pet.ProfileImageUrl = dto.ProfileImageUrl;
        pet.DietaryPreferences = dto.DietaryPreferences;
        pet.AllergiesOrMedicalNotes = dto.AllergiesOrMedicalNotes;
        pet.Notes = dto.Notes;
        pet.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<PetProfileDto>.Ok(MapPetToDto(pet), "Pet profile updated."));
    }

    /// <summary>
    /// Delete a pet profile
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePet(ulong id)
    {
        var pet = await _context.PetProfiles.FindAsync(id);
        if (pet == null)
        {
            return NotFound(ApiResponse<bool>.Fail("Pet profile not found.", 404));
        }

        var userId = GetCurrentUserId();
        if (pet.UserId != userId && !IsAdminOrStaff())
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<bool>.Fail("You cannot delete this pet profile.", 403));
        }

        _context.PetProfiles.Remove(pet);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Pet profile deleted."));
    }

    private static PetProfileDto MapPetToDto(PetProfile p) => new()
    {
        Id = p.Id,
        UserId = p.UserId,
        PetTypeId = p.PetTypeId,
        PetTypeName = p.PetType?.Name ?? string.Empty,
        Name = p.Name,
        Breed = p.Breed,
        Gender = p.Gender,
        DateOfBirth = p.DateOfBirth,
        Weight = p.Weight,
        ProfileImageUrl = p.ProfileImageUrl,
        DietaryPreferences = p.DietaryPreferences,
        AllergiesOrMedicalNotes = p.AllergiesOrMedicalNotes,
        Notes = p.Notes
    };
}
