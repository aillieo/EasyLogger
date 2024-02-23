// -----------------------------------------------------------------------
// <copyright file="TextureAssets.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    internal class TextureAssets
    {
        public static readonly string logo = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAACNklEQVRYCcVX0VGDQBBNHAugA88KZMYG0oFYgdeBWknGCiQdYAXBfz/owOtAOtD3cDfZEAgcZHRnHu/d5nb3eQEmLm8XH4v/jIszDXfosxVQj45zGQiY6IAV8AiMjnMZ4MAXmZqNno6Ncw2szbBctAOPNjHHQIpBK0Cjhihk8aDJIZ5jgENowpkhG9EZODH5XjnHAIcwlKkLoKZA+OY6cJlqgEOd9G7f9bnkR30NUw3cyRBSaTSlPg0pNHEyphrgCWi8qRAO4Ep0+3QkvacpBjzKE2lRgwvRlvQUMpvs0lMM2OPvGs45mk+gPRN9EWuADTPTTP9Sk2pkjWsuSWtYUnuKNeD3pYsAXZl1W9p3gmt/qOtYA/bR0gHaq80lEkGSXviIYgw4VKemQ250n1ST1vjB3hgD3lTy6INZ98lcPnDglegDijFwhcpS0HfzHTTHIsh+5jtPYfkHP8k8hr8CfDKuhUG/EXMCWhPLOQo4PAEy4CD+wgAHFjL16NU8xgBdf0fiUwYq6T2TIuE0Sb60ix5t32Ql9rz37GunEyRqSVbgADiAp/AMNDF0E7LJl+wl3QOFWcfIJ2xeAwHgzdjE0FeQ6UZwAKYOZ5ucF4QDdn2HDNjjnzOcg2tAe+zeCacMJNYp9AaYG9ojQyP2P/l/ATdpBIhKFzO4QG0t9Z586gR4t2qw8FyRS6Pma+gy4LFhC6SykaTPsUlFywwV7LuSSvZPu94DN7KhFA5gYm6UaMBTrQFqRvoDM/NjBzTqqUwAAAAASUVORK5CYII=";
        public static readonly string erroricon = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAADCElEQVRYCe1WTUhUURg9No5mUeTKCmYRIVGQIWqEREi0rKWthIKgpbjubx8UKEE7oXUb6Yc2rYJW0n9oUeRY6pTaDDlaNs57czvnOm9485z3NDNaNB98M/e+7+9857vvzgBV+d8ZqFkHAU2M2U01gVgvV5LPvwVsoVsvKNQhYGhvA7q7gH0uUKDNizebuOYz3AeefQDucPmKuqHS0QVcWwDm2boJ01Fgdi9wmZVbNrJ6m4r/ABYcFs+F6M8iMIKY2QNcJICDGwHisIpzqHP5QOGleNwsJRJlgDwQr4HpZuAKARz6ExDtx4Hri8BisHguFjNOf78x09PG6esrAyGGNKI3y+O4RADrYqKJB+4qZ55VcXVWpo2Nxh0eNhJ3aKjcVvQVCI7jy07gAkFsr8QED2+oJE4AB7YC2wjAHnkd+5LOz8PMztrgwsQE9AaUbMU1AWM/0HRsmYGEdQ581Ab2/q1LKh12YRP7DVobx0EhmUSMa3dszPp476TfV8AoarSSGVEA7E2jBMUkSlQSAXPHxxHntJ3JSesTrKC9YjWKUmBgsSoAvnaQBkUZYyxsMhk4BCKfSgDEkEYTJpEAFBjFgDovpNN/jwEBUGdhI8inUnBGR5EnCDFSiYFibOhhj2IgVgfURjFgCCA7MAAnn19RnHisMIcaUJqKkwhFxoCpR8BbXr/f1Zmig7qltxc7CKChu9sy5beroorzMpp5vPzD9InbFRIF4PNz4HYPMMjEOVHsjcN+19SgrrMT8ZYWxFtbS2dFhWUXte/4s8z4m2ngAbe801aKDmmUpD4yyRMCOMlLiUzUq4DASHMjI3CmpjA3OAg3m7UMyR6nvge+ngVuJIG73L6kVhSxuxZpPwr03ALOs3CDikgEQrSLRiXSfjOVRTNngP6JVYrT1V5k+l5NUhxgRkyc4rXKInWiWKpuvXU91+w8fY6d0z+yc7paWSsDnn8H/2WcPgI0a85+EQti4yHwlJ3f4/KF3x62/l0AyrOrqGLcL16uMT5c839Cf4LquspAlYF/wsAv5L1uQQehuBcAAAAASUVORK5CYII=";
        public static readonly string infoicon = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAADUUlEQVRYCe1UO0hbYRT+TUw0RDEvU/EB9UWDmCZ1EzF2MJMYZ0FXKR262KXdikuh6qyDUHSRUgXp4NYWiw6CliwlFIqPqDRpUgJqgkZz+30XU2Jyb4z10cUDh3vveX3fOff8vxB3cjeBuwn85wkUXRLfhPj70CqoHsp8CXoM/QndhMagBUuhBGyo2OJwOLr6+vraOjo6Gi0Wi0Gn02lOTk5S0Wg0sby8/GNhYeFrIBD4jNhv0Aj0WsRZXV39Ynp6+ks8Hj+W8gj9jGM8kJ3Xgf6os7Pzzfb2digbd21tTRoeHpYWFxezXRLjmQcC7quQuFdbW/syFAr9zkGAYWpqSurp6ZFGRkakZDKZE8I85oMA90VVNKoe/PPR0VGf3W43o7rIVOZUVlYKvV4vbDab0Gq15/yMZd7Y2JgPoS15MIQaAXN9fX2Xz+drVUsmgaKiIgLJT6W43t7eVtTxwGdW8tOmRsDqdrtrDAaD8fT0VKRSqXPKRJPJJEpKSuQJ8Ds7hnnMZx24rYxRkmIlI2xao9HIcy4XZqeZwhGbzWZRUVEhE6GPtkzhN3/NWR1tpi/zXY3AcTgcjjOQnWULi6OwTKK8vFywW6qSnNU5UvLRpvYLwuvr65t7e3tRjUaTM15svSgtLRXd3d3yL8BllBPDvN3d3SiO6wZwfl2WwGEkEvk4MTGxittOLs6u01pcXCxATmxtbYlgMCgvYdrHJ6fGvMnJyVXckp8AfqhGIJ/diGP2bGlp6TvGK+3v78t6cHAg8Xt2dlZqb2+XxsfHJUzgr59x9DOP+QAw5gNRXQ4kJVEoOj8/H29qarK7XK4qdpYed1lZmXwKPB6PsFqt8g5wMth8MTc35+/v73+Lq/kD6oTyESjEV4fCTwcHB2dWVlY2EomE3DG7pPAWjMVi8gSwN8GBgYEZxqNwXSHFz58v9Qwu6wOce6/T6azB2W5sbm62A0iLp8Xr9Tow+qO2trbXOzs77xAbgOYen6z63JfLCu8G3u0PobzhHuOOeDI0NPTe7/cHcSSfw2aAXigE/xcCSoV1MLoaGhpeYUJcOn7nlTT4dRFIg1nwcilwEih0B9IgV3oqdXxrBJTA2c2tEFADvxUC+cBvnMBF4DdKoBBwEND+AUa1M40tjyiVAAAAAElFTkSuQmCC";
        public static readonly string warnicon = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAADbElEQVRYCe1VzUtUURT/vfdGUaQPKPIbdUYJbNQZp9GxJPyWoD8goV1EICQabYNoU1bQtoI2IuGuXavAPmhRhouECoPE1DIjKht1vm+/83wTxow6z1nmgfPeu+ee8zu/e+659wG7sluB7CvgyAbCyCaYsS7DQJ9S+M7vH1li2Q6vDdTh7qsxTB+vwz1Gu20jZBGQy5UPTo1hXr2Dmn6IxRwHhoiXZxdTtxtg+bvaffC4PShVy0DNYRT2tsDHOdcO8WyFGZqG/olRzMVeGGrlWblaG3eo16P4bOi4QKQcO2g7qUA1V9/kdWolob0DUJ5pxIsvw1OhF/UE0Mzk1XYI2PXN0XUMcPUz4ac5KrjwWAVXlAouvVGh8Xw1+QBz0hsEzc0U2G4FXK31qDvajMpELA6EZwFBCM0iHonB24iyDj88tGRcBTsEZO87hi+hG1Hm1RJM/N4i8BbQaIwB1wbRqWtoJ4mMLig7BFytDfD5fSiXRLpcYaGP6wTCM2YhQE6NHpS0+eDnbEYnIlMCDq6q58ZFtBmKuZhIk/WFvwARZW6FORZ7Avr1IbSxV3rose2JyJSA018Ld6AJToQIy5xMAC2yxOQLwNoSNKkI7TLv96OCt2M9R07qlpIJAS4eXTcH0St7bwoTaYzUYyQQ+sAqLLEnOCMERMLA8CC62TOdHAm1TSUTAs5mNzzH/Nx7Akv5TWVCHUEkVqaAKP9FkiY5R78mL8oCbnhpraJuKluyY5R0/umRK+hzluIgePLMVcpK+R0PKqjFlyTwGw7+BczGlDkqK6K7SrBv5BG+0TJpRfL1r2xXAae3GrXtLTzXawxk9yc1/ot5C1q51BHg0ElE5GcsW5T0oX9bAK7GGhyhddMqbEVA5rpu9XPvpfEEWBJYGuNPCAfOAMWngKJzwKoOJX5JH/Hn+OpZ9HC3ujhKmyutkc4iFQ2VaDjhI3tZvZQ/qQQ35IB9ug/MPwFmbrPsbABJmvSRN+N6A6jy1Zi3YyUtKbLVbaUliGnELObSYBvEkc+55QnEn7eDfQJjjzW50U/scejRqInBUaqkNVpuUp3zxfvhKshDfkKZPf4XQbOOnPWCCcRHciyO0ojBEFYXf2KGwzvUjfTEZT3O/Er/EBKFVPm7bcRO751qFV4R6ldqSvJU913LbgX+xwr8AZsH++puLSZoAAAAAElFTkSuQmCC";

        public static Texture2D Base64ToTexture(string base64String)
        {
            Texture2D tex = new Texture2D(1, 1);
            byte[] bytes = Convert.FromBase64String(base64String);
            tex.LoadImage(bytes);
            tex.Apply();
            return tex;
        }

        public static string TextureToBase64(Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            return Convert.ToBase64String(bytes);
        }

#if UNITY_EDITOR
        // [MenuItem("Assets/CopyBase64Info", false, 0)]
        public static void CopyBase64Info()
        {
            Debug.Log(Selection.activeObject);
            Texture2D texture = Selection.activeObject as Texture2D;
            if (texture != null)
            {
                GUIUtility.systemCopyBuffer = TextureToBase64(texture);
            }
        }
#endif
    }
}
