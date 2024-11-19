//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Acts as the "brain" for a pawn. How it receives input essentially.
// Notes: This class is a base class, so it should not be modified, only extended from
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework.PawnManagement
{
    public abstract class PawnController : ScriptableObject
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public abstract void PawnAwake(Pawn _pawn);
        public abstract void PawnUpdate(Pawn _pawn);
        public abstract void PawnFixedUpdate(Pawn _pawn);


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=


        //=-----------------=
        // Mono Functions
        //=-----------------=

        
        //=-----------------=
        // Internal Functions
        //=-----------------=


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}
