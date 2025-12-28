using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public static class Constants
    {
        #region TAGS
        public const string PLAYER_TAG = "Player";
        public const string ANIMAL_TAG = "Animal";
        public const string THROWABLE_TAG = "Throwable";
        public const string PLANTABLE_TAG = "Plantable";
        public const string UNTAGGED_TAG = "Untagged";


        #endregion
        #region TEXTS
        public const string GAME_TITLE = "FEED THOSE \r\nBEASTS!";
        public const string START_BUTTON_TEXT = "PLAY!";
        public const string GAMEOVER_BUTTON_TEXT = "TRY AGAIN";
        public const string GAMEOVER_TEXT = "GAME OVER";
        public const string VICTORY_TEXT = "YOU WIN!";
        public const string VICTORY_BUTTON_TEXT = "PLAY AGAIN!";
        public const string STAMPEDE_TEXT = "STAMPEDE INCOMING!";
        public const string SPAWN_MOOSE_TEXT = "MOOSE INCOMING";
         public const string BOSS_BAR_TEXT = "BOSS HUNGER";
         
        public const string INTRO_TEXT_1 = "Every day, deforestations leave hundreds of aniamls without food";
        public const string INTRO_TEXT_2 = "forcing them to come here, my farm";
        public const string INTRO_TEXT_3 = "in order to avoid chaos, i'll need to take measures";
        public const string INTRO_TEXT_4 = "i'll have to...";
        public const string OUTRO_TEXT_1 = "And so were the aniamls fed that day";
        public const string OUTRO_TEXT_2 = "But the real struggle didn't disappear: the greedy deforestation still shakes inocent animal lifes";
        public const string OUTRO_TEXT_3 = "Some problems are beyond our hands, but remember: you have a voice.";
        public const string OUTRO_TEXT_4 = "Thank you for playing.";



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
        public const string ANIM_BOOL_EAT = "Eat_b";
        public const string ANIM_BODY_VERTICAL = "Body_Vertical_f";



        #endregion

    }

    public enum AnimalStatus
    {
        Running,
        Fetching,
        Stopped
    }

    public enum MusicThemes
    {
        MainMenu,
        InGame,
        Win,
        Lose,
        Boss,
        Introdution
    }

    public enum AnimalType
    {
        Cow,
        Dog,
        Chicken,
        Wolf,
        Moose,
        Doe
    }

    public enum FXTypes
    {
        ClickOnButton,
        AnimalFed,
        Shot,
        LoseLife,
        DogBone,

        WrongFood,
        Points,
        SelectItem

    }

    public enum FoodTypes
    {
        Carrot,
        Beef,
        Bone
    }
    public enum Levels
    {
        Level1 = 1,
        Level2,
        Level3,
        Level4,
        Level5
    }
    [Serializable]
    public class FoodItemTransparent
    {
        public FoodTypes foodTypes;
        public GameObject goFood;

    }
    [Serializable]
    public class MusicItem
    {
        public MusicThemes musicThemes;
        public AudioClip audioClip;
    }
    [Serializable]
    public class FXItem
    {
        public FXTypes fxType;
        public AudioClip audioClip;
    }
}
