/**
 * PawHaven BD - "My Pet" Profile & Personalized Care Controller
 */

class PetsController {
  constructor() {
    this.container = document.getElementById("my-pets-grid");
    this.recommendationsContainer = document.getElementById("pet-recommendations-grid");
    this.addPetModal = document.getElementById("add-pet-modal");
    this.addPetForm = document.getElementById("add-pet-form");

    this.init();
  }

  async init() {
    if (this.addPetForm) {
      this.addPetForm.addEventListener("submit", (e) => this.handleCreatePet(e));
    }
    await this.loadPets();
  }

  async loadPets() {
    if (!this.container) return;

    if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
      this.container.innerHTML = `
        <div class="col-span-full bg-white rounded-2xl border border-slate-200/80 p-8 text-center">
          <p class="text-sm text-slate-500 mb-4">Please sign in to view and create your pet profiles.</p>
          <a href="login.html?redirect=my-pets.html" class="inline-block px-5 py-2.5 rounded-xl bg-teal-600 text-white font-bold text-sm">Sign In</a>
        </div>
      `;
      return;
    }

    this.container.innerHTML = `
      <div class="col-span-full py-12 text-center">
        <div class="w-8 h-8 border-4 border-teal-500 border-t-transparent rounded-full animate-spin mx-auto mb-3"></div>
        <p class="text-sm text-slate-500">Loading your pets...</p>
      </div>
    `;

    try {
      const res = await window.api.getMyPets();
      if (!res.success || !res.data || res.data.length === 0) {
        this.container.innerHTML = `
          <div class="col-span-full bg-white rounded-2xl border border-dashed border-teal-300 p-10 text-center">
            <div class="w-16 h-16 bg-teal-50 rounded-full flex items-center justify-center mx-auto mb-3 text-teal-600">
              <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"/></svg>
            </div>
            <h3 class="text-lg font-bold text-slate-800 mb-1">No Pet Profiles Added Yet</h3>
            <p class="text-sm text-slate-500 mb-6 max-w-sm mx-auto">Create a profile for your cat, dog, bird, or rabbit to receive personalized nutrition and veterinary-backed product advice!</p>
            <button onclick="window.petsController.openAddModal()" class="px-5 py-2.5 rounded-xl bg-teal-600 hover:bg-teal-700 text-white font-bold text-sm shadow-md transition">
              + Add My First Pet
            </button>
          </div>
        `;
        return;
      }

      const pets = res.data;
      this.container.innerHTML = pets.map(pet => {
        const petIcon = pet.petTypeName?.toLowerCase() === 'cat' ? '🐱' :
                        pet.petTypeName?.toLowerCase() === 'dog' ? '🐶' :
                        pet.petTypeName?.toLowerCase() === 'bird' ? '🦜' :
                        pet.petTypeName?.toLowerCase() === 'rabbit' ? '🐰' : '🐾';

        return `
          <div class="bg-white rounded-2xl border border-slate-200/80 p-6 shadow-sm hover:shadow-md transition relative flex flex-col justify-between">
            <div>
              <div class="flex items-start justify-between gap-3 mb-4">
                <div class="flex items-center gap-3">
                  <div class="w-12 h-12 rounded-xl bg-teal-50 border border-teal-100 flex items-center justify-center text-2xl">
                    ${petIcon}
                  </div>
                  <div>
                    <h3 class="font-extrabold text-slate-900 text-lg">${pet.name}</h3>
                    <p class="text-xs text-teal-700 font-semibold uppercase tracking-wider">${pet.breed || pet.petTypeName}</p>
                  </div>
                </div>

                <button onclick="window.petsController.deletePet(${pet.id})" class="text-slate-400 hover:text-red-500 p-1 rounded-lg" title="Delete Profile">
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"/></svg>
                </button>
              </div>

              <div class="grid grid-cols-2 gap-2 text-xs py-3 border-y border-slate-100 mb-4">
                <div>
                  <span class="text-slate-400 block">Gender:</span>
                  <span class="font-bold text-slate-700 capitalize">${pet.gender || 'Unknown'}</span>
                </div>
                <div>
                  <span class="text-slate-400 block">Weight:</span>
                  <span class="font-bold text-slate-700">${pet.weight ? `${pet.weight} kg` : 'N/A'}</span>
                </div>
                ${pet.dateOfBirth ? `
                  <div class="col-span-2">
                    <span class="text-slate-400 block">Birth Date:</span>
                    <span class="font-bold text-slate-700">${pet.dateOfBirth} (${pet.ageInMonths ? Math.floor(pet.ageInMonths / 12) + ' yrs' : ''})</span>
                  </div>
                ` : ''}
              </div>

              ${pet.allergiesOrMedicalNotes ? `
                <div class="bg-amber-50/60 border border-amber-200/60 rounded-xl p-3 text-xs text-amber-900 mb-4">
                  <strong class="font-bold block mb-0.5">Health & Dietary Note:</strong>
                  ${pet.allergiesOrMedicalNotes}
                </div>
              ` : ''}
            </div>

            <button onclick="window.petsController.loadRecommendationsForPet(${pet.id})" class="w-full py-2.5 px-3 rounded-xl bg-slate-50 hover:bg-teal-50 text-teal-800 font-bold text-xs transition border border-teal-200/60 flex items-center justify-center gap-1.5">
              <span>View Recommended Products</span>
              <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
            </button>
          </div>
        `;
      }).join("");

      // Automatically trigger recommendation engine on first pet
      if (pets.length > 0) {
        this.loadRecommendationsForPet(pets[0].id);
      }
    } catch (err) {
      this.container.innerHTML = `<div class="col-span-full py-8 text-center text-red-500">Failed to load pets.</div>`;
    }
  }

  async loadRecommendationsForPet(petId) {
    if (!this.recommendationsContainer) return;

    this.recommendationsContainer.innerHTML = `
      <div class="col-span-full py-8 text-center">
        <div class="w-6 h-6 border-2 border-teal-500 border-t-transparent rounded-full animate-spin mx-auto mb-2"></div>
        <p class="text-xs text-slate-500">Calculating tailored recommendations...</p>
      </div>
    `;

    try {
      const res = await window.api.getRecommendations(8, petId);
      if (res.success && res.data && res.data.length > 0) {
        this.recommendationsContainer.innerHTML = res.data.map(p => window.renderProductCard(p)).join("");
      } else {
        this.recommendationsContainer.innerHTML = `<div class="col-span-full py-6 text-center text-slate-400 text-sm">No specific recommendations available.</div>`;
      }
    } catch (e) {
      this.recommendationsContainer.innerHTML = `<div class="col-span-full py-6 text-center text-red-500 text-sm">Failed to generate recommendations.</div>`;
    }
  }

  openAddModal() {
    if (this.addPetModal) this.addPetModal.classList.remove("hidden");
  }

  closeAddModal() {
    if (this.addPetModal) this.addPetModal.classList.add("hidden");
  }

  async handleCreatePet(e) {
    e.preventDefault();
    const name = document.getElementById("pet-name")?.value.trim();
    const petTypeId = parseInt(document.getElementById("pet-type-id")?.value);
    const breed = document.getElementById("pet-breed")?.value.trim();
    const gender = document.getElementById("pet-gender")?.value;
    const dob = document.getElementById("pet-dob")?.value || null;
    const weight = document.getElementById("pet-weight")?.value ? parseFloat(document.getElementById("pet-weight").value) : null;
    const notes = document.getElementById("pet-notes")?.value.trim();

    if (!name || !petTypeId) {
      window.showToast("Please provide your pet's name and type.", "warning");
      return;
    }

    const payload = {
      name,
      petTypeId,
      breed,
      gender,
      dateOfBirth: dob,
      weight,
      allergiesOrMedicalNotes: notes,
      notes
    };

    const res = await window.api.createPet(payload);
    if (res.success) {
      window.showToast(`${name}'s profile has been created!`, "success");
      this.closeAddModal();
      this.addPetForm.reset();
      await this.loadPets();
    } else {
      window.showToast(res.message || "Failed to create pet profile.", "error");
    }
  }

  async deletePet(id) {
    if (!confirm("Are you sure you want to remove this pet profile?")) return;
    const res = await window.api.deletePet(id);
    if (res.success) {
      window.showToast("Pet profile deleted.", "info");
      await this.loadPets();
    } else {
      window.showToast(res.message || "Could not delete pet profile.", "error");
    }
  }
}

document.addEventListener("DOMContentLoaded", () => {
  if (document.getElementById("my-pets-grid")) {
    window.petsController = new PetsController();
  }
});
