console.log("map.js loaded");

document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM ready");

    const regionSelect = document.getElementById("regionSelect");
    const mapDiv = document.getElementById("map");

    if (!regionSelect || !mapDiv) {
        console.warn("regionSelect or map div not found");
        return;
    }

    // zapobieganie wielokrotnej inicjalizacji
    if (mapDiv._leaflet_id != null) {
        console.warn("Map already initialized.");
        return;
    }

    const map = L.map("map").setView([52.1, 19.3], 6); // centrum Polski
    console.log("Map initialized");

    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    const options = regionSelect.querySelectorAll("option");
    console.log("Found options:", options.length);
    console.log("regionSelect HTML:", regionSelect.innerHTML);

    options.forEach(option => {
        const lat = parseFloat(option.dataset.lat.replace(',', '.'));
        const lng = parseFloat(option.dataset.lng.replace(',', '.'));
        const name = option.textContent;

        if (isNaN(lat) || isNaN(lng)) {
            console.warn("Invalid coords for:", name, lat, lng);
            return;
        }

        const marker = L.marker([lat, lng]).addTo(map);
        marker.bindPopup(name);

        marker.on("click", () => {
            regionSelect.value = option.value;
            console.log("Selected:", name);

            // Znajdź formularz i wyślij
            const form = document.querySelector("form");
            if (form) {
                form.submit();
            }
        });

        });
    });
});
