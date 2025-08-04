# Everyone to the Hackathon

Project for a special course by [True Engineering](https://www.trueengineering.ru/), written in C#.

## Description

An IT company decided to form their dream teams (DreamTeam). To achieve this, a junior HR manager (HRManager) proposed to the experienced HR director (HRDirector) to organize a hackathon (Hackathon).

20 juniors (Junior) and 20 team leads (TeamLead) are invited to the hackathon. Developers are asked to collaborate and by the end of the event submit a list of colleagues (Wishlist) they would like to work with in a team. Each junior ranks all 20 team leads in order of preference (most preferred first). Team leads do the same for the juniors. All lists must contain 20 unique entries — juniors rank team leads, and team leads rank juniors.

These lists are passed to the junior HR manager, who promised to come up with an innovative strategy to form the dream teams. Each team must consist of one TeamLead + one Junior. The HRManager will send the final team list to the HRDirector.

All would be simple, but the experienced HRDirector proposed a mathematical way to evaluate the "harmony" of the formed teams. Each developer gets a satisfaction score based on the position of their partner in their wishlist. For example, if a team lead is paired with their top-choice junior, they get 20 satisfaction points; second choice – 19, and so on down to 1. Juniors are scored the same way.

Then, the HRDirector calculates the **harmonic mean** of all participants’ satisfaction scores. This value represents the harmony of the event. The HRManager’s main objective is to maximize this harmony when forming the teams.

The hackathon is held multiple times. The final result is the **average of all harmonic means** across all events.

---

## Task 1: OOP

Implement a console application. Preference lists should be generated randomly. Participant lists are read from files: `Juniors20.csv` and `Teamleads20.csv` (available in [this repository](https://github.com/georgiy-sidorov/CSHARP_2024_NSU/tree/main)).

The hackathon should be held 1000 times. The app must print the harmony score for each event and the overall average.

> The application should not download files from GitHub during execution. Download them in advance and place them next to your source code.

---

## Task 2: .NET Generic Host

Refactor the app to use the .NET Generic Host. Separate responsibilities into the following components:

* A class for conducting a single hackathon;
* A class implementing the HRManager;
* A class implementing the HRDirector;
* The strategy used by the HRManager.

---

## Task 3: Testing

Implement the following unit tests:

* **Wishlist Generation**:

  * The list length must match the number of team leads/juniors;
  * A predetermined participant must be present in the list.

* **HRManager**:

  * The number of teams must match the expected count;
  * Given a fixed set of preferences, the strategy must produce a predetermined team distribution;
  * The strategy must be invoked exactly once.

* **HRDirector**:

  * Test harmonic mean calculation: e.g., identical values should return that same value;
  * Test harmonic mean for specific examples: e.g., 2 and 6 should yield 3;
  * Given known preferences and team assignments, the harmony score should match expected output.

* **Hackathon Execution**:

  * A hackathon with fixed participants and preferences should yield a known harmony score.

---

## Task 4: Database

Connect the project to a database (any RDBMS is allowed).

* Each hackathon should have a unique identifier;
* Hackathon details (participants, preferences, teams, harmony score) must be stored in the DB.

The app should support:

* Running one hackathon with random preferences and storing the result in the DB;
* Displaying participants, team assignments, and harmony score by hackathon ID;
* Calculating and printing the average harmony score across all hackathons in the DB.

Write additional tests using **SQLite InMemory**. Cover:

* Writing event data to the DB;
* Reading event data from the DB;
* Calculating and storing the average harmonic score.

---

## Task 5: Web Services

Now the hackathon has 5 juniors and 5 team leads (`Juniors5.csv` and `Teamleads5.csv`). The satisfaction score formula changes accordingly: 5 points for top choice, 4 for second, ..., down to 1.

Files: [available here](https://github.com/georgiy-sidorov/CSHARP_2024_NSU/tree/main)

Implement the system using separate app instances in Docker containers:

* One for each team lead;
* One for each junior;
* One for the HRManager;
* One for the HRDirector.

Participants send their preferences via **HTTP** to the HRManager, who determines the team assignments.

The HRManager sends all preferences and final teams via **HTTP** to the HRDirector.

The HRDirector calculates harmony, prints it to the console, and stores the result in the DB (as in Task 4).

Only one hackathon is run.

Use a single `docker-compose.yml` file to run the entire system with one command.

---

## Task 6: MassTransit and RabbitMQ

The HRDirector announces the start of the event by sending a special message via **RabbitMQ** (may include the hackathon ID). Juniors and team leads publish their preferences via RabbitMQ.

The HRManager receives preferences, forms the teams, and sends the result via **HTTP** to the HRDirector.

The HRDirector:

* Receives teams via HTTP;
* Receives preferences via RabbitMQ;
* Calculates harmony based on the teams;
* Stores harmony and preference data in the DB.

Everything must be orchestrated via a single `docker-compose.yml` file, just like in the previous task.

The event is held 10 times.

Interaction scheme 😊:

![image](https://github.com/user-attachments/assets/82e2fa9a-cd31-4d77-8a96-169cb6fee61c)

**Important!**
All events must be processed immediately when possible — no fixed timeouts, no `Thread.Sleep(1000)` or similar.
