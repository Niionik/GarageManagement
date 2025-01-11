# Garage Management System

## 1. Wprowadzenie

Projekt GarageManagement to aplikacja do zarządzania garażami i samochodami. Kompleksowe rozwiązanie biznesowe stworzone z myślą o właścicielach warsztatów samochodowych, flotach firmowych oraz indywidualnych kolekcjonerach pojazdów. System odpowiada na rosnące potrzeby rynku w zakresie efektywnego zarządzania pojazdami i ich serwisowaniem.

### Dlaczego GarageManagement?

- **Optymalizacja kosztów**
  - Śledzenie historii napraw pozwala na lepsze planowanie budżetu
  - Monitorowanie terminów przeglądów zapobiega droższym naprawom
  - Analiza kosztów napraw pomaga w podejmowaniu decyzji o wymianie pojazdu

- **Zwiększenie efektywności**
  - Szybki dostęp do pełnej historii każdego pojazdu
  - Automatyczne przypomnienia o zbliżających się przeglądach
  - Łatwe zarządzanie wieloma lokalizacjami (garażami)

- **Bezpieczeństwo danych**
  - Różne poziomy dostępu dla różnych ról użytkowników
  - Pełna kontrola nad tym, kto ma dostęp do informacji
  - Bezpieczne przechowywanie dokumentacji

### Dla kogo?

1. **Warsztaty samochodowe**
   - Zarządzanie historią napraw klientów
   - Planowanie terminów serwisowych
   - Śledzenie części zamiennych

2. **Firmy z flotą pojazdów**
   - Kontrola kosztów utrzymania floty
   - Zarządzanie wieloma lokalizacjami
   - Raportowanie i analiza wydatków

3. **Kolekcjonerzy i entuzjaści**
   - Dokumentacja historii pojazdów
   - Zarządzanie kolekcją
   - Śledzenie wartości pojazdów

4. **Wypożyczalnie samochodów**
   - Zarządzanie flotą wynajmowanych pojazdów
   - Śledzenie terminów przeglądów
   - Kontrola stanu technicznego

## 2. Wymagania systemowe

- **System operacyjny**: Windows, macOS, Linux
- **.NET SDK**: 8.0 lub nowszy
- **Baza danych**: SQL Server
- **Przeglądarka**: Chrome, Firefox, Edge

## 3. Instalacja i uruchomienie

1. **Klonowanie repozytorium**:
   ```bash
   git clone https://github.com/your-repo/garage-management.git
   cd garage-management
   ```

2. **Konfiguracja bazy danych**:
   - Upewnij się, że SQL Server jest uruchomiony
   - Zaktualizuj `appsettings.json` z odpowiednim connection stringiem
   - Wykonaj migracje: `dotnet ef database update`

3. **Uruchomienie aplikacji**:
   ```bash
   dotnet run
   ```
   Aplikacja będzie dostępna pod adresem `http://localhost:7123`

## 4. Dostęp do systemu

### Użytkownicy testowi
- **Administrator**:
  - Email: admin@example.com
  - Hasło: AdminPassword123!
- **Zwykły użytkownik**:
  - Email: owner@example.com
  - Hasło: Password123!

### Poziomy dostępu
- **Zwykli użytkownicy**: Zarządzanie własnymi pojazdami i garażami
- **Administratorzy**: Pełny dostęp do systemu, w tym zarządzanie użytkownikami

## 5. Funkcjonalności systemu

### Zarządzanie użytkownikami
- Rejestracja i logowanie
- Edycja profilu użytkownika
- Zarządzanie uprawnieniami (dla administratorów)
- Zmiana hasła

### Zarządzanie garażami
- Dodawanie i edycja garaży
- Przypisywanie pojazdów
- Zarządzanie lokalizacjami
- Kopiowanie garaży (dla administratorów)

### Zarządzanie pojazdami
- Dodawanie i edycja pojazdów
- Śledzenie stanu technicznego
- Historia napraw i serwisów
- Informacje o oponach i kołach

### Historia napraw
- Rejestrowanie napraw
- Koszty serwisów
- Terminy przeglądów
- Statystyki i raporty

## 5.1. Szczegółowy opis funkcjonalności

### Formularze
- **Rejestracja**: Tworzenie nowych kont użytkowników z walidacją danych (email, imię, nazwisko, hasło)
- **Logowanie**: Bezpieczny dostęp do aplikacji z wykorzystaniem ASP.NET Core Identity
- **Dodawanie samochodu**: Kompleksowy formularz z walidacją danych technicznych pojazdu
- **Edycja samochodu**: Aktualizacja danych pojazdu z zachowaniem historii zmian
- **Dodawanie garażu**: Tworzenie nowych lokalizacji z mapowaniem i opisem
- **Edycja garażu**: Zarządzanie istniejącymi lokalizacjami
- **Dodawanie naprawy**: Szczegółowy formularz dokumentacji serwisowej
- **Edycja naprawy**: Modyfikacja historii serwisowej z zachowaniem audytu zmian

### Encje systemu
- **User (Owner)**
  - Zarządzanie danymi użytkowników
  - System ról i uprawnień
  - Historia aktywności
  - Preferencje użytkownika

- **Car**
  - Pełne dane techniczne pojazdu
  - Status i stan techniczny
  - Historia przeglądów i napraw
  - Informacje o oponach i częściach

- **Garage**
  - Dane lokalizacyjne
  - Przypisane pojazdy
  - Statystyki wykorzystania
  - Historia zmian

- **Maintenance**
  - Szczegółowa historia napraw
  - Koszty serwisowe
  - Dokumentacja techniczna
  - Przypomnienia i alerty

### System autoryzacji
- **Użytkownicy standardowi**
  - Zarządzanie własnymi pojazdami
  - Dostęp do historii napraw
  - Podstawowe raporty

- **Administratorzy**
  - Pełne zarządzanie systemem
  - Zaawansowane raporty
  - Klonowanie danych
  - Zarządzanie użytkownikami

### WebAPI
- **Operacje CRUD dla pojazdów**
  - Dodawanie nowych pojazdów (Create)
  - Pobieranie danych pojazdów (Read)
  - Aktualizacja informacji (Update)
  - Usuwanie pojazdów (Delete)
- **Dokumentacja API w Swagger**
- **Zabezpieczenia JWT**
- **Rate limiting i caching**

## 6. Architektura systemu

### Warstwy aplikacji
- **Prezentacja (MVC)**
  - Kontrolery: Obsługa żądań HTTP
  - Widoki: Razor Pages z Bootstrap 5
  - Modele: ViewModels dla formularzy

- **Logika biznesowa**
  - Serwisy do zarządzania danymi
  - Walidacja biznesowa
  - Autoryzacja i uprawnienia

- **Dostęp do danych**
  - Entity Framework Core
  - SQL Server
  - Migracje bazy danych

### Bezpieczeństwo
- ASP.NET Core Identity
- Role i uprawnienia (RBAC)
- Ochrona przed CSRF i XSS
- Szyfrowanie danych

## 7. Plany rozwoju

### Planowane funkcjonalności
- Integracja z systemami płatności
- Aplikacja mobilna
- System powiadomień (SMS/Email)
- Rozbudowane raporty i analityka

### Obszary doskonalenia
- Optymalizacja wydajności
- Rozszerzenie API
- Dodatkowe integracje
- Ulepszenia UX/UI

## 8. Uwagi końcowe

- Przed uruchomieniem upewnij się, że wszystkie zależności są zainstalowane
- Regularnie wykonuj kopie zapasowe bazy danych
- Aktualizuj dokumentację przy wprowadzaniu zmian
- W razie problemów sprawdź logi aplikacji

## 9. Autorzy

- **Patrycja Opałacz** (14968)


- **Konrad Kopacz**

_Copyright © 2025 - Wszelkie prawa zastrzeżone_


