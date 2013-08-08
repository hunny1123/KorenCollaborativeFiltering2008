using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

namespace Factorization_neighborhood
{
    class RecommenderSystem
    {
        Dictionary<int, Dictionary<int, short>> r;
        Dictionary<int, double> user_bias;
        Dictionary<int, double> movie_bias;
        Dictionary<int, int> num_rated_movie;
        Dictionary<int, int> num_rated_user;
        Dictionary<int, double> avg_user_rating;


        /// <summary>
        /// Metaparameters
        /// </summary>
        
        float regularization_term = .015f;
        
        double avg_rating;

        public RecommenderSystem(IEnumerable<string> Training)//, IEnumerable<string> Testing)
        {
            r = new Dictionary<int, Dictionary<int, short>>();
            user_bias = new Dictionary<int, double>();
            movie_bias = new Dictionary<int, double>();
            num_rated_movie = new Dictionary<int, int>();
            num_rated_user = new Dictionary<int, int>();
            avg_user_rating = new Dictionary<int, double>();
            ProcessInput(Training);
            CalculateBiases();
        }

        private void ProcessInput(IEnumerable<string> files)
        {
            StreamReader reader;
            string line;
            int num_movies = files.Count();
            int user;
            int movie;
            short rating;
            int num_users;
            foreach (string file in files)
            {
                Console.WriteLine(file);
                using (reader = new StreamReader(file))
                {
                    movie = int.Parse(reader.ReadLine().Split(':')[0]) - 1;
                    if (!r.ContainsKey(movie))
                    {
                        r[movie] = new Dictionary<int, short>();
                        movie_bias[movie] = 0;
                        num_rated_movie[movie] = 0;
                    }
                    while ((line = reader.ReadLine()) != null)
                    {
                       user = int.Parse(line.Split(',')[0]) -1;
                       rating = short.Parse(line.Split(',')[1]);
                       if (!avg_user_rating.ContainsKey(user))
                       {
                           num_rated_user[user] = 0;
                           avg_user_rating[user] = 0;
                           user_bias[user] = 0;
                       }
                       num_rated_user[user] += 1;
                       num_rated_movie[movie] += 1;
                       avg_user_rating[user] += rating;
                       r[movie][user] = rating;
                       avg_rating += rating;

                    }
                }
            }
            avg_rating /= num_rated_user.Count;

        }

        private void CalculateBiases()
        {
            //new way
            foreach (int movie in num_rated_movie.Keys)
            {
                foreach (int user in r[movie].Keys )
                {
                    user_bias[user] += r[movie][user] - avg_rating;
                    movie_bias[movie] += r[movie][user] - avg_rating;
                }
            }

            foreach (int movie in movie_bias.Keys.ToList())
            {
                movie_bias[movie] /= num_rated_movie[movie];
            }

            foreach (int user in user_bias.Keys.ToList())
            {
                user_bias[user] /= num_rated_user[user];
            }


        }
        public double PredictValue(int user, int movie)
        {
            return avg_rating + user_bias[user] + movie_bias[movie];
        }
        
    }
}
