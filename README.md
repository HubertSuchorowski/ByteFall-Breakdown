# ByteFall - Retro FPS Roguelite 

**ByteFall** to dynamiczna gra zręcznościowa typu roguelite utrzymana w stylistyce retro. Projekt został stworzony przez zespół **SzpontGames** na konkurs „Platynowy Indeks Politechniki Świętokrzyskiej 2026”.

## 🛠 Mój wkład w projekt (Hubert Suchorowski)
Jako **Junior Unity Developer** oraz Kapitan Zespołu, byłem odpowiedzialny za zaprojektowanie i zaimplementowanie wszystkich kluczowych systemów rozgrywki oraz architektury technicznej (z wyłączeniem generacji terenu, systemu walki i animacji).

### Kluczowe systemy:
* **System Walki i AI:** Implementacja przeciwników oparta na technologii **NavMesh**, zapewniająca płynną nawigację w dynamicznie zmieniającym się środowisku.
* **Integracja AI (LLM & TTS):** Stworzenie  bossa AI z pełną integracją **REST API**. System komunikuje się z serwerem (FastAPI), wykorzystując model **Google Gemini 2.5 Flash** do generowania dialogów oraz **ElevenLabs** do syntezy mowy w czasie rzeczywistym.
* **Optymalizacja (Object Pooling):** Wykorzystanie wzorca projektowego **UnityEngine.Pool** do zarządzania falami przeciwników i pociskami, co pozwoliło na zachowanie wysokiej wydajności (FPS) przy dużej skali potyczek.
* **Architektura Data-Driven:** Zastosowanie **Scriptable Objects** do konfiguracji balansu gry, statystyk broni oraz definicji fal przeciwników, co umożliwiło łatwą modyfikację parametrów bez ingerencji w kod źródłowy.

## Technologie
* **Silnik:** Unity 6.3+
* **Język:** C#
* **AI/API:** FastAPI (Python), Google Gemini API, ElevenLabs TTS
* **Narzędzia:** Plastic SCM (Kontrola wersji), NavMesh, Object Pooling API

## Struktura Repozytorium
To repozytorium zawiera wybrane skrypty mojego autorstwa, prezentujące architekturę systemów gry:
* `/Scripts/AI/` – Logika przeciwników i integracja z API Gemini.
* `/Scripts/Systems/Wave/` – System fal, Object Pooling oraz WaveManager.
* `/Scripts/Systems/Player/` – System gracza oraz PlayerManager.
* `/Scripts/Data/` – Definicje Scriptable Objects.

## Dokumentacja
Pełna dokumentacja projektowa (Breakdown) zawierająca opisy koncepcyjne oraz graficzne znajduje się w folderze `/Docs`.

---
Projekt "ByteFall" został przygotowany na konkurs **Platynowy Indeks Politechniki Świętokrzyskiej 2026**. 
* **Prawa majątkowe:** Autorskie prawa majątkowe do projektu zostały przekazane Organizatorowi Konkursu zgodnie z podpisanymi zobowiązaniami.
* **Cel repozytorium:** Niniejsze repozytorium pełni rolę **portfolio technicznego**. Udostępnione fragmenty kodu służą wyłącznie demonstracji moich umiejętności programistycznych i warsztatu technicznego (tzw. do użytku osobistego/edukacyjnego).
* **Autorstwo:** Zachowuję niezbywalne autorskie prawa osobiste do stworzonych przeze mnie rozwiązań programistycznych opisanych w sekcji "Mój wkład".
