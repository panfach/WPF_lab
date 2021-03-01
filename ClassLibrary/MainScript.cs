// ------------------------------- Variant 3.2 ------------------------------- // 


using System;
using System.Numerics;

namespace Lab
{
    class MainScript
    {
        static void Main()
        {
            V3DataCollection fileDataCollection = new V3DataCollection("test1.txt");
            Console.WriteLine(fileDataCollection.ToLongString("F1"));

            V3MainCollection mainCollection = new V3MainCollection();
            mainCollection.AddDefaults(); 
            Console.WriteLine(mainCollection.ToString("F3"));



            // Первая отладка
            Console.WriteLine("Minimum amount of items in Data = " + mainCollection.MinItems);
            Console.WriteLine("Maximum distance of items = " + mainCollection.MaxDist);
            Console.WriteLine("Repetitive items : \n");
            foreach (var item in mainCollection.GetRepetitiveItems)
            {
                Console.WriteLine(item);
            }

       

            // Для второй отладки создается отдельный mainCollection
            V3MainCollection mainCollection2 = new V3MainCollection();
            mainCollection2.Add(new V3DataCollection("test2.txt"));
            mainCollection2.Add(new V3DataCollection("test3.txt"));
            Console.WriteLine("\n\n\n" + mainCollection2.ToString("F3"));

            
            Console.WriteLine("Minimum amount of items in Data = " + mainCollection2.MinItems);
            Console.WriteLine("Maximum distance of items = " + mainCollection2.MaxDist);
            Console.WriteLine("Repetitive items : ");
            foreach (var item in mainCollection2.GetRepetitiveItems)
            {
                Console.WriteLine(item);
            }
        }

        static void MainCollectionNearest(V3MainCollection mainCollection, Vector2 point, string number)
        {
            Console.WriteLine($"{number} point: {point}");
            foreach (V3Data item in mainCollection)
            {
                foreach (Vector2 coord in item.Nearest(point))
                {
                    Console.Write(coord + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
