using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesseractBoxCreator.Model
{
    public class BoxesList : List<BoxItem>
    {
        public IEnumerable<BoxItem> GetSorted()
        {
            return SortItems(this);
        }

        public IEnumerable<BoxItem> GetForPage(int CurrentPage)
        {
            return SortItems(this.Where(x => x.Page == CurrentPage));
        }

        protected IEnumerable<BoxItem> SortItems(IEnumerable<BoxItem> list)
        {
            return list;//.OrderBy(x => x.Page).ThenByDescending(x => x.Y).ThenBy(x => x.X).ThenByDescending(x => x.Y2).ThenBy(x => x.X2).ThenBy(x => x.Letter);
        }
    }
}
