import styles from './UserLevel.module.css';

export type Level = 'Новичок' | 'Читатель' | 'Критик' | 'Эксперт' | 'Легенда';

interface Props {
  initials: string;
  booksRead: number;
  size?: number;
}

function getLevel(books: number): { level: Level; color: string; gradient: string; emoji: string; next: number } {
  if (books >= 100) return { level: 'Легенда',  color: '#F59E0B', gradient: 'linear-gradient(135deg,#F59E0B,#EF4444,#8B5CF6)', emoji: '👑', next: Infinity };
  if (books >= 50)  return { level: 'Эксперт',  color: '#8B5CF6', gradient: 'linear-gradient(135deg,#6D28D9,#8B5CF6,#A78BFA)', emoji: '💎', next: 100 };
  if (books >= 25)  return { level: 'Критик',   color: '#0369A1', gradient: 'linear-gradient(135deg,#0369A1,#0EA5E9,#38BDF8)', emoji: '🏆', next: 50 };
  if (books >= 10)  return { level: 'Читатель', color: '#059669', gradient: 'linear-gradient(135deg,#047857,#059669,#34D399)', emoji: '📚', next: 25 };
  return                  { level: 'Новичок',  color: '#94A3B8', gradient: 'linear-gradient(135deg,#64748B,#94A3B8,#CBD5E1)', emoji: '🌱', next: 10 };
}

export const UserLevel = ({ initials, booksRead, size = 56 }: Props) => {
  const { level, gradient, emoji, next } = getLevel(booksRead);
  const pct = next === Infinity ? 100 : Math.min((booksRead / next) * 100, 100);
  const circumference = 2 * Math.PI * ((size / 2) - 3);
  const offset = circumference * (1 - pct / 100);

  return (
    <div className={styles.wrap} style={{ width: size + 8, height: size + 8 }} title={`Level: ${level} (${booksRead} books read)`}>
      {/* SVG ring */}
      <svg width={size + 8} height={size + 8} style={{ position: 'absolute', top: 0, left: 0 }}>
        <defs>
          <linearGradient id="lvl-grad" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" stopColor="#F59E0B" />
            <stop offset="50%" stopColor="#EF4444" />
            <stop offset="100%" stopColor="#8B5CF6" />
          </linearGradient>
        </defs>
        {/* Track */}
        <circle cx={(size + 8) / 2} cy={(size + 8) / 2} r={(size / 2) - 3}
          fill="none" stroke="var(--border)" strokeWidth="3" />
        {/* Progress */}
        <circle cx={(size + 8) / 2} cy={(size + 8) / 2} r={(size / 2) - 3}
          fill="none" stroke="url(#lvl-grad)" strokeWidth="3"
          strokeDasharray={circumference}
          strokeDashoffset={offset}
          strokeLinecap="round"
          transform={`rotate(-90 ${(size + 8) / 2} ${(size + 8) / 2})`}
          style={{ transition: 'stroke-dashoffset 0.6s ease' }}
        />
      </svg>

      {/* Avatar */}
      <div className={styles.avatar} style={{ width: size - 4, height: size - 4, background: gradient, fontSize: size * 0.32 }}>
        {initials}
      </div>

      {/* Level badge */}
      <span className={styles.badge}>{emoji}</span>
    </div>
  );
};
