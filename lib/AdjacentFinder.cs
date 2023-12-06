using Lib.Interfaces;

namespace Lib;

/// <summary>
/// Find adjacent items on a map.
/// Each item must have a X, Y, Width and Height, so the AdjacentFinder can locate items that are
/// adjacent vertically, horizontally or diagonally.
/// In the following example, all #'s are adjacent:
/// #.##
/// .#..
/// .#..
/// </summary>
/// <typeparam name="T">IMapItem containing a X, Y, Width and Height</typeparam>
public static class AdjacentFinder<T> where T : IMapItem 
{
    public static IEnumerable<T> FindAdjacentItems(IEnumerable<T> items, int x, int y)
    {
        return items.Where(item => 
                item.Y <= y + 1 
                && item.Y + item.Height - 1 >= y - 1
                && item.X <= x + 1 
                && item.X + item.Width - 1 >= x - 1
            ).ToList();
    }

    public static IEnumerable<T> FindAdjacentItems(IEnumerable<T> items, IMapItem target)
    {
        return items.Where(item => 
                item.Y <= target.Y + target.Height 
                && item.Y + item.Height - 1 >= target.Y - 1
                && item.X <= target.X + target.Width 
                && item.X + item.Width - 1 >= target.X - 1
            ).ToList();
    }
}
