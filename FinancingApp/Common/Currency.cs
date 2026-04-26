namespace FinancingApp.Common
{
    public readonly struct Currency(int amount = 0)
    {
        private readonly int _amount = amount;

        public int Value() => _amount;

        public override string ToString()
        {
            return $"CHF {_amount / 100}.{Math.Abs(_amount % 100):D2}";
        }

        public static Currency operator +(Currency left, Currency right)
        {
            return new Currency(left._amount + right._amount);
        }

        public static Currency operator -(Currency left, Currency right)
        {
            return new Currency(left._amount - right._amount);
        }

        public static Currency operator *(Currency left, double multiplier)
        {
            return new Currency((int)(left._amount * multiplier));
        }

        public static Currency operator /(Currency left, double divisor)
        {
            return new Currency((int)(left._amount / divisor));
        }

        public static implicit operator Currency(int amount)
        {
            return new Currency(amount * 100);
        }

        public static implicit operator Currency(double amount)
        {
            return new Currency((int)(amount * 100));
        }

        public static bool TryParse(string amount, out Currency currency)
        {
            try
            {
                const string chf = "CHF ";
                amount = amount.StartsWith(chf) ? amount[chf.Length..] : amount;
                currency = Parse(amount);
            } catch
            {
                currency = new Currency(0);
                return false;
            }

            return true;
        }

        public static Currency Parse(string amount)
        {
            // clean up comma and delimiting characters
            amount = amount.Replace(",", ".");
            amount = amount.Replace("'", string.Empty);

            bool isNegative = amount.StartsWith("-");

            var parts = amount.TrimStart('-').Split('.');
            if (parts.Length > 2)
            {
                throw new ArgumentException("Invalid currency input: may contain at most 1 dot");
            }

            if(parts.Length == 0)
            {
                return new Currency(0);
            }

            int totalCents = 0;

            if (int.TryParse(parts[0], out int franks))
            {
                totalCents = franks * 100;
            } else
            {
                throw new ArgumentException("Invalid currency input: can not convert to swiss franks");
            }

            if(parts.Length < 2)
            {
                return new Currency(isNegative ? -totalCents : totalCents);
            }

            if (int.TryParse(parts[1], out int cents))
            {
                // check for implicit dangling zeroes, e.g. 12.5 should be 12.50
                if (parts[1].Length == 1)
                {
                    cents *= 10;
                }

                if (cents < 0 || cents > 99)
                {
                    throw new ArgumentException("Invalid currency input: cents must be between 0 and 99");
                }

                totalCents += cents;
            }
            else
            {
                throw new ArgumentException("Invalid currency input: can not convert to swiss franks");
            }

            return new Currency(isNegative ? -totalCents : totalCents);
        }
    }
}
