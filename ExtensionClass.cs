/* ExtensionClass.cs
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

namespace FntEditor
{
    public static class ExtensionClass
    {
        public static byte SetBit(this byte value, int newBit, int position)
        {
            if (position >= 8 || position < 0)
                throw new ArgumentOutOfRangeException("position", "The argument is not in the allowed ranged for this type.");
            if (newBit != 0 && newBit != 1)
                throw new ArgumentException("The bit value only can be 1 or 0.", "newBit");
            return (byte)((value & (value ^ (1 << position))) | (newBit << position));
        }

        public static int GetBit(this byte value, int position)
        {
            if (position >= 8 || position < 0)
                throw new ArgumentOutOfRangeException("position", "The argument is not in the allowed ranged for this type.");
            return (value & 1 << position) >> position;
        }
    }
}
