﻿using System.Linq;
using SabberStoneCore.Enums;

namespace SabberStoneCore.Model
{
    public class Deck : Zone<IPlayable>
    {
        public int StartingCards { get; set; } = 30;

        public Deck(Game game, Controller controller) : base(game, controller, Zone.DECK)
        {
        }

        public void Fill()
        {
            var cards = Game.FormatType == FormatType.FT_STANDARD ? Controller.Standard : Controller.Wild;
            var cardsToAdd = StartingCards - Count;

            Game.Log(LogLevel.INFO, BlockType.PLAY, "Deck", $"Deck[{Game.FormatType}] from {Controller.Name} filling up with {cardsToAdd} random cards.");
            while (cardsToAdd > 0)
            {
                var card = Util<Card>.Choose(cards);
                if (cards.Count(c => c == card) >= card.MaxAllowedInDeck) continue;
                Entity.FromCard(Controller, card, null, this);
                cardsToAdd--;
            }
        }

        public void Shuffle(int times = 100)
        {
            // no need to shuffle something that has no or only one entity ...
            if (Count < 2)
            {
                return;
            }

            Game.Log(LogLevel.INFO, BlockType.PLAY, "Deck", $"Deck[{Game.FormatType}] from {Controller.Name} shuffling ({times}x).");
            for (var i = 0; i < times; i++)
            {
                Swap(Random, Random);
            }
        }
    }
}
