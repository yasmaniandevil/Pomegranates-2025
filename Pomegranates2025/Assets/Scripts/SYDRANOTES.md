// Singleton: A static self reference that exists inside of a non-static script that allows OTHER scripts to access an instance of something important
// Such as the player, or a game manager, without needing a reference to it first!
// Basic OOP principles (see SOLID) is a great way to avoid using the game manager and having an instance be accessible and used by everyone if it doesn't need to be

// Singleton responsible for managing the state of our game
// State: Current condition of object or system and also decides the behavior of the object or system
// State Pattern: When we use the concepts of state within code!
// - Each state's code is self-contained
// - Allows us to modify the behavior independently
// - Changes to one state will not impact other states

// State pattern can be implemented with a Finite State Machine, limiting to the number of states we want to program.
// 1) Define each state of the machine
// 2) Definte the transitions between states
// 3) Select the initial state

// I) Context
// Creates instances of concrete states to use when game is running 
// Passes data to currently active state
// Context Derives from MonoBehavior Class (Start, Update)

// II) Abstract State
// A blueprint of "prototype" class for concrete states 
// Sets / Defines common methods use in all concrete states
// Type Definition of the CurrentState Variable in the Context

// III) Concrete State
// Individual states themselves self-contained with their own specific behavior
// Define own properties and methods WITH definitions of the Abstract State items