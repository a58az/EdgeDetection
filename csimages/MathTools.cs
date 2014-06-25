namespace csimages
{
    internal class MathTools
    {
        public static bool IsPowerOf2(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public static int RoundUpToPow2(int width)
        {
            width--;
            width |= width >> 1;
            width |= width >> 2;
            width |= width >> 4;
            width |= width >> 8;
            width |= width >> 16;
            width++;
            return width;
        }

        public static int Pow2(int numberOfBits)
        {
            return 1 << numberOfBits;
        }

        public static int Log2(int number)
        {
            if (number <= 65536)
            {
                if (number <= 256)
                {
                    if (number <= 16)
                    {
                        if (number <= 4)
                        {
                            if (number <= 2)
                            {
                                if (number <= 1)
                                    return 0;
                                return 1;
                            }
                            return 2;
                        }
                        if (number <= 8)
                            return 3;
                        return 4;
                    }
                    if (number <= 64)
                    {
                        if (number <= 32)
                            return 5;
                        return 6;
                    }
                    if (number <= 128)
                        return 7;
                    return 8;
                }
                if (number <= 4096)
                {
                    if (number <= 1024)
                    {
                        if (number <= 512)
                            return 9;
                        return 10;
                    }
                    if (number <= 2048)
                        return 11;
                    return 12;
                }
                if (number <= 16384)
                {
                    if (number <= 8192)
                        return 13;
                    return 14;
                }
                if (number <= 32768)
                    return 15;
                return 16;
            }

            if (number <= 16777216)
            {
                if (number <= 1048576)
                {
                    if (number <= 262144)
                    {
                        if (number <= 131072)
                            return 17;
                        return 18;
                    }
                    if (number <= 524288)
                        return 19;
                    return 20;
                }
                if (number <= 4194304)
                {
                    if (number <= 2097152)
                        return 21;
                    return 22;
                }
                if (number <= 8388608)
                    return 23;
                return 24;
            }
            if (number <= 268435456)
            {
                if (number <= 67108864)
                {
                    if (number <= 33554432)
                        return 25;
                    return 26;
                }
                if (number <= 134217728)
                    return 27;
                return 28;
            }
            if (number <= 1073741824)
            {
                if (number <= 536870912)
                    return 29;
                return 30;
            }
            return 31;
        }
    }
}

