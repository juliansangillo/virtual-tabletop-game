using System;
using System.Collections;
using UnityEngine;

namespace NaughtyBikerGames.SDK.Interpolation.Interfaces {
    /**
    * Performs Linear Interpolation, aka Lerp, between two provided values over a duration of time in seconds. A Lerp will
    * update a value over time starting from the source and until it reaches the destination. A callback is provided so the
    * caller can use it to update their values with the ever-changing value. All Lerps return an IEnumerator so that it can
    * be run as a coroutine.
    * 
    * Rather than simply assigning one value to another, this makes the transition more smooth. It can be helpful when 
    * the values being modified have a graphical component and you want the changes to appear more gradual and less 
    * instant. The changes are animated in a linear way. That is, if you plot the changes on a graph over time, it will
    * be a straight line. A Lerp does NOT allow values to be eased in and/or out.
    *
    * @author Julian Sangillo \<https://github.com/juliansangillo\>
    * @version 3.0
    * @since 3.0
    */
	public interface ILerpable {
        /**
        * Lerps between two Vector3s over a given duration of time and makes a callback with an update on each frame. Since
        * this returns an IEnumerator, it can be used as a coroutine.
        *
        * @param source The starting value of the Lerp as a Vector3
        * @param destination The end value of the Lerp as a Vector3
        * @param duration The duration of the Lerp in seconds
        * @param update A callback that gets called each frame the Lerp iterates
        *
        * @return An IEnumerator that is used to start a coroutine
        */
        IEnumerator Lerp(Vector3 source, Vector3 destination, float duration, Action<Vector3> update);
	}
}