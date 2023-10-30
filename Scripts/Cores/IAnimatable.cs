using System.Collections;
namespace MochaLib.Cores
{
    public interface IAnimatable
    {
        void Animate(int animationHash);
        IEnumerator WaitUntilAnimationEnd();
    }
}
