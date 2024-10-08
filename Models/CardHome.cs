﻿using Models.Movies;

namespace Models;

public class CardHome
{

    public int Id { get;  set; }
    public string ImagePath { get;  set; }
    public string Assessment { get;  set; }
    public bool Concluded { get; set; }

    public CardHome(int id, string image, string assessments)
    {
        Id = id;
        ImagePath = image;
        Assessment = assessments;
    }


    public static List<CardHome> GetCardsHome(List<CardHome> assessments)
    {
        var cardsHome = new List<CardHome>();
        foreach (var assessment in assessments)
        {
            cardsHome.Add(assessment);
        }

        return cardsHome;
    }
}
