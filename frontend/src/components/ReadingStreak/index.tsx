import styles from './ReadingStreak.module.css';

interface Props {
  days?: number;
}

export const ReadingStreak = ({ days = 7 }: Props) => (
  <div className={styles.streak} title={`${days}-day reading streak`} role="status" aria-label={`${days} day reading streak`}>
    <span className={styles.flame} aria-hidden="true">🔥</span>
    <span className={styles.count}>{days}</span>
    <span className={styles.label}>day streak</span>
  </div>
);
