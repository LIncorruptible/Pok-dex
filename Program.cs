using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Caching;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using static Pokédex.CU;

/// <summary>
/// Classe Principale du projet.
/// </summary>
namespace Pokédex
{
    class Program
    {
        /// <summary>
        /// Fonction récupérant tous les types de Pokemon présent dans l'API.
        /// </summary>
        /// <param name="CollectionPokemons"></param>
        /// <returns></returns>
        public static List<string> recuperationListeTypes(List<List<Pokemon>> CollectionPokemons) 
        {
            List<string> ListeTypesRecupere = new List<string>();

            foreach (List<Pokemon> ListePokemonG in CollectionPokemons)
            {
                List<Pokemon> PokemonGRecupere = new List<Pokemon>();

                foreach (Pokemon pokemon in ListePokemonG)
                {
                    foreach(string type in pokemon.types)
                    {
                        if (!ListeTypesRecupere.Contains(type)) ListeTypesRecupere.Add(type);
                    }
                }
            }

            return ListeTypesRecupere;
        }

        /// <summary>
        /// Fonction qui extrait la chaine de caractères Json d'un objet API.
        /// </summary>
        /// <param name="API"></param>
        /// <returns></returns>
        public static string extraireJsonPokemon(API API)
        {
            System.Net.WebClient clientPokemon = new System.Net.WebClient();
            return clientPokemon.DownloadString(API.url);
        }

        /// <summary>
        /// Fonction de désérialisation d'un Json en Pokemon.
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static Pokemon convertionJsonPokemon(string Json)
        {
            return JsonConvert.DeserializeObject<Pokemon>(Json);
        }


        /// <summary>
        /// Fonction désérialisant une Liste d'API en une Liste de Pokemon selon des indices de parcours dans l'API.
        /// </summary>
        /// <param name="pokemonsAPI"></param>
        /// <param name="pokemonsG"></param>
        /// <param name="pokemons"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public static void extraireGeneration(List<API> pokemonsAPI, ref List<Pokemon> pokemonsG, ref List<List<Pokemon>> pokemons, int A, int B)
        {

            for (int indice = A; indice < B; indice++)
            {
                pokemonsG.Add(JsonConvert.DeserializeObject<Pokemon>(extraireJsonPokemon(pokemonsAPI[indice])));
                ConsoleUtility.compteur++;
            }
            pokemons.Add(pokemonsG);
        }

        /// <summary>
        /// Récupère sous forme d'un string le tableau de type(s) d'un Pokemon.
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public static string recuperationTypePokemon(Pokemon pokemon)
        {
            string Types = "";

            int i = 0;

            foreach (string type in pokemon.types)
            {
                Types += (i == 0) ? $" {type}" : $", {type}";
                i++;
            }

            return Types;
        }

        /// <summary>
        /// Affiche par Génération, tous les pokemons d'une Collection (ici Liste de Liste de Pokemon).
        /// </summary>
        /// <param name="CollectionPokemons"></param>
        /// <param name="afficherType"></param>
        public static void afficherCollectionPokemon(List<List<Pokemon>> CollectionPokemons, bool afficherType)
        {
            int i = 1;

            foreach(List<Pokemon> pokemonG in CollectionPokemons)
            {
                Console.WriteLine("+--------------+");
                Console.WriteLine($"| Génération {i} |");
                Console.WriteLine("+--------------+");
                i++;

                foreach(Pokemon pokemon in pokemonG)
                {
                    Console.WriteLine($"> ID : {pokemon.id} | Nom : {pokemon.name.en} {((afficherType == true) ? $" | Type : {recuperationTypePokemon(pokemon)}" : "")}");
                }
            }
        }


        /// <summary>
        /// Affiche tous les Pokemons d'une Liste donnée.
        /// </summary>
        /// <param name="ListePokemonG"></param>
        /// <param name="numGeneration"></param>
        public static void afficherGenerationPokemon(List<Pokemon> ListePokemonG, int numGeneration)
        {
            Console.WriteLine("+--------------+");
            Console.WriteLine($"| Génération {numGeneration} |");
            Console.WriteLine("+--------------+");

            foreach (Pokemon pokemon in ListePokemonG)
            {
                Console.WriteLine($"> ID : {pokemon.id} | Nom : {pokemon.name.en}");
            }
        }


        /// <summary>
        /// Ordonne par ordre croissant de génration une Collection de Pokemon (ici Liste de Liste de Pokemon).
        /// </summary>
        /// <param name="CollectionPokemons"></param>
        public static void triageCollectionPokemonParGeneration(ref List<List<Pokemon>> CollectionPokemons)
        {
            CollectionPokemons = CollectionPokemons.OrderBy(ListePokemonG => ListePokemonG[0].id).ToList();
        }


        /// <summary>
        /// Regroupe par Type tous les Pokemons d'un type voulu sous forme d'une Liste de Pokemon.
        /// </summary>
        /// <param name="CollectionPokemons"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Pokemon> regroupeParType(List<List<Pokemon>> CollectionPokemons, string type)
        {
            List<Pokemon> ListePokemonDuType = new List<Pokemon>();

            foreach(List<Pokemon> ListePokemonG in CollectionPokemons)
            {
                foreach(Pokemon pokemon in ListePokemonG)
                {
                    if (pokemon.types.Contains(type)) ListePokemonDuType.Add(pokemon);
                }
            }

            return ListePokemonDuType;
        }

        /// <summary>
        /// Retourne un Pokemon aléatoire selon une Liste de Pokemon donnée.
        /// </summary>
        /// <param name="ListePokemonDuType"></param>
        /// <returns></returns>
        public static Pokemon randomPokemonDuType(List<Pokemon> ListePokemonDuType)
        {
            return ListePokemonDuType[new Random().Next(ListePokemonDuType.Count)];
        }


        /// <summary>
        /// Récupération d'un Pokemon pour chaque Type par génération.
        /// </summary>
        /// <param name="CollectionPokemons"></param>
        /// <param name="Types"></param>
        /// <returns></returns>
        public static List<List<Pokemon>> recuperationChaqueTypeParGeneration(List<List<Pokemon>> CollectionPokemons, List<string> Types)
        {
            List<List<Pokemon>> PokemonRecupere = new List<List<Pokemon>>();

            foreach(List<Pokemon> ListePokemonG in CollectionPokemons)
            {
                List<Pokemon> ListePokemonGRecupere = new List<Pokemon>();

                foreach(string type in Types)
                {
                    ListePokemonGRecupere.Add(randomPokemonDuType(regroupeParType(CollectionPokemons, type)));
                }

                PokemonRecupere.Add(ListePokemonGRecupere);
            }

            return PokemonRecupere;
        }


        /// <summary>
        /// Récupération des Pokemons d'un Type donné pour chaque Generation.
        /// </summary>
        /// <param name="CollectionPokemons"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<List<Pokemon>> recuperationParTypeParGeneration(List<List<Pokemon>> CollectionPokemons, string type)
        {
            List<List<Pokemon>> ListePokemonRecupere = new List<List<Pokemon>>();

            foreach(List<Pokemon> ListePokemonG in CollectionPokemons)
            {
                List<Pokemon> PokemonGRecupere = new List<Pokemon>();
                foreach(Pokemon pokemon in ListePokemonG)
                {
                    List<string> types = pokemon.types.ConvertAll(t => t.ToLowerInvariant());
                    if (types.Contains(type)) PokemonGRecupere.Add(pokemon);
                }
                ListePokemonRecupere.Add(PokemonGRecupere);
            }

            return ListePokemonRecupere;
        }


        /// <summary>
        /// Récuparation des Pokemons d'une Génération.
        /// </summary>
        /// <param name="CollectionPokemons"></param>
        /// <param name="numGeneration"></param>
        /// <returns></returns>
        public static List<Pokemon> recuperationPokemonParGeneration(List<List<Pokemon>> CollectionPokemons, int numGeneration)
        {
            return CollectionPokemons[numGeneration];
        }


        /// <summary>
        /// Donne la moyenne de poids en Kg d'un type de Pokemon.
        /// </summary>
        /// <param name="CollectionPokemons"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static double moyennePoidsPokemonParType(List<List<Pokemon>> CollectionPokemons, string type)
        {
            double sommePoids = 0;

            List<Pokemon> ListePokemonDuType = regroupeParType(CollectionPokemons, type);

            foreach(Pokemon pokemon in ListePokemonDuType)
            {
                sommePoids += pokemon.weight;
            }

            return (sommePoids / ListePokemonDuType.Count) / 10;
        }

        /// <summary>
        /// Affiche les statistiques complètes d'un Pokemon
        /// </summary>
        /// <param name="pokemon"></param>
        public static void afficherStatistiquePokemon(Pokemon pokemon)
        {
            Console.WriteLine($"ID : {pokemon.id}");
            Console.WriteLine($"Nom : {pokemon.name}");
            Console.WriteLine($"Types : {recuperationTypePokemon(pokemon)}");
            Console.WriteLine($"Taille : {pokemon.height} cm");
            Console.WriteLine($"Poids : {pokemon.weight / 10} Kg");
            Console.WriteLine($"Genre : {pokemon.genus}");
        }

        /// <summary>
        /// Fonction qui exécute l'Interface graphique pour exécuter les différentes fonctions prévues aux exercices.
        /// </summary>
        /// <param name="CollectionPokemons"></param>
        public static void afficherIntegraliteParExercices(List<List<Pokemon>> CollectionPokemons)
        {
            while (true)
            {
                string[] text = System.IO.File.ReadAllLines("../../pokemon.txt");

                Console.WriteLine();

                foreach(string line in text)
                {
                    Console.WriteLine($"\t{line}");
                }

                string[] questions =
                {
                    "Afficher la liste des Pokémons : Numéro et nom",
                    "Afficher un Pokémons de chaque type pour chaque génération",
                    "Afficher tous les Pokémons d’un type (au choix)",
                    "Afficher tous les Pokémons de la génération 3",
                    "Afficher la moyenne des poids des Pokémons de types Acier"
                };

                Console.WriteLine("\nQuelle question voulez-vous lancer ? Notez le numéro :\n");

                for (int i = 0; i < 6; i++) Console.WriteLine($"> {i + 1} - " + ((i != 5) ? $"Question {i + 1} : " + questions[i] : "Quitter"));

                int choix = 0;

                do
                {
                    try
                    {
                        Console.Write("\nVotre choix (1-6) : ");
                        choix = Convert.ToInt32(Console.ReadLine());
                    } catch
                    {
                        Console.WriteLine("Donnez un choix valide !");
                    }
                } while (choix == 0);

                Console.Clear();

                switch (choix)
                {
                    case 1:
                        afficherCollectionPokemon(CollectionPokemons, false);        
                        break;
                    case 2:
                        afficherCollectionPokemon(recuperationChaqueTypeParGeneration(CollectionPokemons, recuperationListeTypes(CollectionPokemons)), true);
                        break;
                    case 3:
                        Console.WriteLine("Renseignez le type de Pokemon désiré selon la liste qui suit :");

                        int indice = 0;

                        foreach(string type in recuperationListeTypes(CollectionPokemons))
                        {
                            if (indice % 3 == 0)
                            {
                                Console.Write("\n");
                                indice = 0;
                            }
                            Console.Write((indice == 0) ? $"{type}" : $", {type}");
                            indice++;
                        }

                        Console.Write("\n\nVotre choix > ");

                        string chosenType = Convert.ToString(Console.ReadLine());

                        afficherCollectionPokemon(recuperationParTypeParGeneration(CollectionPokemons, chosenType.ToLower()), true);
                        break;
                    case 4:
                        afficherGenerationPokemon(recuperationPokemonParGeneration(CollectionPokemons, 3), 3);
                        break;
                    case 5:
                        Console.WriteLine($"La moyenne des poids des Pokemons de type Acier est de {moyennePoidsPokemonParType(CollectionPokemons, "Steel").ToString(".00")}Kg.");
                        break;
                    case 6:
                        System.Environment.Exit(0);
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Appuyez sur une touche pour continuer ...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        /// <summary>
        /// Fonction qui met à jour la barre de progréssion prévue au chargement de l'API.
        /// </summary>
        public static void updateProgressBar()
        {
            ConsoleUtility.WriteProgressBar(0);
            while (ConsoleUtility.compteur < 890)
            {
                ConsoleUtility.WriteProgressBar(true);
            }
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green; // Définit la couleur du texte du terminal sur Vert.

            System.Net.WebClient client = new System.Net.WebClient();
            string jsonAPI = client.DownloadString("https://tmare.ndelpech.fr/tps/pokemons"); //Récupère le JSON

            List<API> API = JsonConvert.DeserializeObject<List<API>>(jsonAPI); //Désérialise le json des APIS en une liste d'objets API

            // Liste comportant l'intégralité des Pokemons par génération.
            List<List<Pokemon>> Pokemons = new List<List<Pokemon>>();

            // Liste des Pokemons selon leur Génération.
            List<Pokemon> PokemonsG1 = new List<Pokemon>();
            List<Pokemon> PokemonsG2 = new List<Pokemon>();
            List<Pokemon> PokemonsG3 = new List<Pokemon>();
            List<Pokemon> PokemonsG4 = new List<Pokemon>();
            List<Pokemon> PokemonsG5 = new List<Pokemon>();
            List<Pokemon> PokemonsG6 = new List<Pokemon>();
            List<Pokemon> PokemonsG7 = new List<Pokemon>();
            List<Pokemon> PokemonsG8 = new List<Pokemon>();

            // List des Thread pour les 8 génrations de Pokemon.
            List<Thread> ThreadExtractionGenerationPokemon = new List<Thread>(8);

            // Thread générant chaque génération ( 1 Thread par génération )
            ThreadExtractionGenerationPokemon.Add(new Thread(() => extraireGeneration(API, ref PokemonsG1, ref Pokemons, 0, 151)));
            ThreadExtractionGenerationPokemon.Add(new Thread(() => extraireGeneration(API, ref PokemonsG2, ref Pokemons, 151, 251)));
            ThreadExtractionGenerationPokemon.Add(new Thread(() => extraireGeneration(API, ref PokemonsG3, ref Pokemons, 251, 386)));
            ThreadExtractionGenerationPokemon.Add(new Thread(() => extraireGeneration(API, ref PokemonsG4, ref Pokemons, 386, 493)));
            ThreadExtractionGenerationPokemon.Add(new Thread(() => extraireGeneration(API, ref PokemonsG5, ref Pokemons, 493, 649)));
            ThreadExtractionGenerationPokemon.Add(new Thread(() => extraireGeneration(API, ref PokemonsG6, ref Pokemons, 649, 721)));
            ThreadExtractionGenerationPokemon.Add(new Thread(() => extraireGeneration(API, ref PokemonsG7, ref Pokemons, 721, 802)));
            ThreadExtractionGenerationPokemon.Add(new Thread(() => extraireGeneration(API, ref PokemonsG8, ref Pokemons, 802, 898)));


            // Thread pour la barre de progression suivant le chargement de l'API. 
            Console.WriteLine("Chargement de l'API : ");
            Thread progressBar = new Thread(() => updateProgressBar());
            progressBar.Start();

            // Lancement des Thread
            foreach (Thread ThreadGenerationPokemon_i in ThreadExtractionGenerationPokemon)
            {
                ThreadGenerationPokemon_i.Start();
            }

            // Fin de vie des Thread
            foreach (Thread ThreadGenerationPokemon_i in ThreadExtractionGenerationPokemon)
            {
                ThreadGenerationPokemon_i.Join();
            }

            progressBar.Join();

            // Trie la Collection de Pokemon ( ici liste de liste ) par ordre croissant de génération.
            triageCollectionPokemonParGeneration(ref Pokemons);
            Console.WriteLine("\nTriage de la Collection par Génération [DONE]");


            Console.WriteLine("Appuyez sur une touche pour continuer ...");
            Console.ReadKey();
            Console.Clear();

            // Lance l'interface GUI qui implémente touts les exercices.
            afficherIntegraliteParExercices(Pokemons);

            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }
    }
}       