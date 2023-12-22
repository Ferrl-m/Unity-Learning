using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress {

    public event EventHandler<OnProgressChangeddEventArgs> OnProgressChanged;
    public class OnProgressChangeddEventArgs : EventArgs {
        public float progressNormalized;
    }
}
