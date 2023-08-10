using System;

namespace LibPostalNet
{
    [Flags]
    public enum AddressComponents : int
    {
        NONE = 0,
        ANY = (1 << 0),
        NAME = (1 << 1),
        HOUSE_NUMBER = (1 << 2),
        STREET = (1 << 3),
        UNIT = (1 << 4),
        LEVEL = (1 << 5),
        STAIRCASE = (1 << 6),
        ENTRANCE = (1 << 7),
        CATEGORY = (1 << 8),
        NEAR = (1 << 9),

        TOPONYM = (1 << 13),
        POSTAL_CODE = (1 << 14),
        PO_BOX = (1 << 15),
        ALL = ((1 << 16) - 1)
    }
}
