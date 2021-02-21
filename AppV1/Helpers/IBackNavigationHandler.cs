using System;

namespace AppV1.Helpers
{
    public interface IBackNavigationHandler
    {
        event EventHandler<bool> OnPageCanGoBackChanged;

        void GoBack();
    }
}
