using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Photino.NET;

namespace PhotinoX.App;

partial class PhotinoApp
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ThrowIfDisposingOrDisposed()
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _state) != States.NotDisposed, this);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowApplicationAlreadyCreated()
    {
        throw new InvalidOperationException($"Cannot create more than one {typeof(PhotinoApp).FullName} instance.");
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static PhotinoWindow ThrowMainWindowNotCreated()
    {
        throw new InvalidOperationException("MainWindow is not created yet.");
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowMainWindowNotConfigured()
    {
        throw new InvalidOperationException("No main window configured.");
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowApplicationAlreadyRunning()
    {
        throw new InvalidOperationException($"The {typeof(PhotinoApp).FullName} application is already running.");
    }
}