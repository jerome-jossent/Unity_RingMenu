using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math_JJ
{

    public class Cercle
    {
        public Vector2 O;
        public float r;
    }

    public class Droite
    {
        public float a;
        public float b;
    }

    public class Intersections2D
    {
        public Vector2 intersection_1;
        public Vector2 intersection_2;
    }

    public class Intersections3D
    {
        public Vector3 intersection_1;
        public Vector3 intersection_2;
    }


    //Equation d'un cercle  (1) : (x - Ox)² + (y - Oy)² = c.r ²
    //Equation d'une droite (2) : y = a * x + b
    //(2) dans (1) : (x - Ox)² +            (a * x + b - Oy)² = r²
    //g = (a * x + b - Oy)² = (a * x + b - Oy) * (a * x + b - Oy)
    //g = a * a * x * x + a * x * b - a * x * Oy + b * a * x + b * b - b * Oy - Oy * a * x - Oy * b + Oy * Oy;
    //g = x * x * (a * a) + x * (a * b - a * Oy + b * a - Oy * a) + (b * b - b * Oy - Oy * b + Oy * Oy);
    //g = x * x * (a * a) + x * 2 * a * (b - Oy) + (b - Oy) * (b - Oy);
    //(2) dans (1) : x * x + Ox * Ox - 2 * x * Ox +    x * x * (a * a) + x * 2 * a * (b - Oy) + (b - Oy) * (b - Oy) = r * r
    //r * r = x * x + x * (- 2 * Ox) + Ox * Ox + x * x * (a * a) + x * 2 * a * (b - Oy) + (b - Oy) * (b - Oy);
    //r * r= x * x * (1 + a * a) + x * (-2 * Ox + 2 * a * (b - Oy)) + Ox * Ox + (b - Oy) * (b - Oy);
    //x * x * (1 + a * a) + x * (-2 * Ox + 2 * a * (b - Oy)) + Ox * Ox + (b - Oy) * (b - Oy) - r * r= 0;
    //Equation de la forme a * x²+ b * x + c = 0
    public static Intersections2D _Intersections2D_DroiteCoupantUnCercle(Cercle c, Droite d)
    {
        if (d.a != float.MinValue && d.a != float.MaxValue)
        {
            float a = d.a;
            float b = d.b;
            float Ox = c.O.x;
            float Oy = c.O.y;
            float r = c.r;

            float _a = (1 + a * a);
            float _b = (-2 * Ox + 2 * a * (b - Oy));
            float _c = Ox * Ox + (b - Oy) * (b - Oy) - r * r;
            Vector2? X12 = _TrinomeDu2ndDegres(_a, _b, _c);
            if (X12 == null)
            {
                Debug.Log($"Aucun point de la droite y = {d.a} * x + {d.b} ne coupe le cercle de rayon {c.r} et de centre {c.O}");
                return null;
            }

            Vector2 x12 = (Vector2)X12;
            //trouve les Y avec l'équation de la droite
            float Y1 = a * x12.x + b;
            float Y2 = a * x12.y + b;

            return new Intersections2D()
            {
                intersection_1 = new Vector2(x12.x, Y1),
                intersection_2 = new Vector2(x12.y, Y2)
            };
        }
        else
        {
            return new Intersections2D()
            {
                intersection_1 = new Vector2(0, c.r),
                intersection_2 = new Vector2(0, -c.r)
            };
        }
    }

    public static Intersections3D _Intersections3D_DroiteCoupantUnCercle(Cercle c, Droite d)
    {
        if (d.a != float.MinValue && d.a != float.MaxValue)
        {
            float a = d.a;
            float b = d.b;
            float Ox = c.O.x;
            float Oy = c.O.y;
            float r = c.r;

            float _a = (1 + a * a);
            float _b = (-2 * Ox + 2 * a * (b - Oy));
            float _c = Ox * Ox + (b - Oy) * (b - Oy) - r;
            Vector2? X12 = _TrinomeDu2ndDegres(_a, _b, _c);
            if (X12 == null)
                return null;

            Vector2 x12 = (Vector2)X12;
            //trouve les Y avec l'équation de la droite
            float Y1 = a * x12.x + b;
            float Y2 = a * x12.y + b;

            return new Intersections3D()
            {
                intersection_1 = new Vector3(x12.x, 0, Y1),
                intersection_2 = new Vector3(x12.y, 0, Y2)
            };
        }
        else
        {
            return new Intersections3D()
            {
                intersection_1 = new Vector3(0, 0, c.r),
                intersection_2 = new Vector3(0, 0, -c.r)
            };
        }
    }
    
    // a*x² + b*x + c = 0
    public static Vector2? _TrinomeDu2ndDegres(float a, float b, float c)
    {
        // delta = b²-4*a*c
        float d = b * b - 4 * a * c;
        if (d < 0)
            return null;
        //x1 = (-b - d^0.5) / 2a
        //x2 = (-b + d^0.5) / 2a
        float x1 = (-b - Mathf.Pow(d, 0.5f)) / (2 * a);
        float x2 = (-b + Mathf.Pow(d, 0.5f)) / (2 * a);
        return new Vector2(x1, x2);
    }

    public static Vector2 _Intersection2D_DroiteCoupantUnCercle(Cercle c, Droite d, float angle)
    {
        Intersections2D i12 = _Intersections2D_DroiteCoupantUnCercle(c, d);

        //i1 ou i2 ==> celui qui est le plus proche du point sans marge ??
        float Bx = c.r * Mathf.Sin(angle / 180 * Mathf.PI);
        float By = c.r * Mathf.Cos(angle / 180 * Mathf.PI);
        Vector2 b = new Vector2(Bx, By);

        if (Vector2.Distance(b, i12.intersection_1) < Vector2.Distance(b, i12.intersection_2))
            return i12.intersection_1;
        else
            return i12.intersection_2;
    }

    public static Vector3 _Intersection3D_DroiteCoupantUnCercle(Cercle c, Droite d, float angle)
    {
        Intersections3D i12 = _Intersections3D_DroiteCoupantUnCercle(c, d);

        //i1 ou i2 ==> celui qui est le plus proche du point sans marge ??
        float Bx = c.r * Mathf.Sin(angle / 180 * Mathf.PI);
        float By = c.r * Mathf.Cos(angle / 180 * Mathf.PI);
        Vector3 b = new Vector3(Bx, 0, By);

        if (Vector3.Distance(b, i12.intersection_1) < Vector3.Distance(b, i12.intersection_2))
            return i12.intersection_1;
        else
            return i12.intersection_2;
    }

}