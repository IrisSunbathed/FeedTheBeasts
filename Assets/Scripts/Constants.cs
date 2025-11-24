using System;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public static class Constants
    {
        #region TAGS
        public const string PLAYER_TAG = "Player";
        public const string ANIMAL_TAG = "Animal";
        #endregion
        #region TEXTS
        public const string GAME_TITLE = "FEED THE \r\nBEASTS!";
        public const string START_BUTTON_TEXT = "PLAY!";
        public const string GAMEOVER_BUTTON_TEXT = "TRY AGAIN";
        public const string GAMEOVER_TEXT = "GAME OVER";
        #endregion
        #region AXIS
        public const string HORIZONTAL_AXIS = "Horizontal";
        public const string VERTICAL_AXIS = "Vertical";
        #endregion
        #region ANIMATION VARIABLES
        public const string ANIM_BOOL_DEATH = "Death_b";
        public const string ANIM_INT_DEATHTYPE = "DeathType_int";
        public const string ANIM_INT_IDLE = "Animation_int";
        public const string ANIM_FLOAT_SPEED = "Speed_f";


        #endregion




    }

    public enum FoodTypes
    {
        Carrot,
        Beef
    }
    [Serializable]
    public class FoodItemTransparent
    {
        public FoodTypes foodTypes;
        public GameObject goFood;

    }
}
