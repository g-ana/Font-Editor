/* IndexedPropertyChangedEventArg.cs
 * Copyright © 2014 Juhász, Ádám L.
 * This file is part of Font Pattern Editor.
 *
 * Font Pattern Editor is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Font Pattern Editor is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Font Pattern Editor.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FntCore
{
    public class IndexedPropertyChangedEventArg: PropertyChangedEventArgs
    {
        object[] indexes;

        public IndexedPropertyChangedEventArg(string propertyName, params object[] indexes)
            : base(propertyName)
        {
            this.indexes = indexes;
        }

        public object[] ChangedIndexes { get { return indexes; } }
    }
}
