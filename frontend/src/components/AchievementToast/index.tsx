import { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import styles from './AchievementToast.module.css';

export interface Achievement {
  icon: string;
  title: string;
  desc: string;
  duration?: number; // ms, default 5000
}

interface Props {
  achievement: Achievement | null;
  onClose: () => void;
}

export const AchievementToast = ({ achievement, onClose }: Props) => {
  const duration = achievement?.duration ?? 5000;

  useEffect(() => {
    if (!achievement) return;
    const t = setTimeout(onClose, duration);
    return () => clearTimeout(t);
  }, [achievement, duration, onClose]);

  return (
    <AnimatePresence>
      {achievement && (
        <motion.div
          className={styles.toast}
          initial={{ opacity: 0, y: 60, scale: 0.9 }}
          animate={{ opacity: 1, y: 0, scale: 1 }}
          exit={{ opacity: 0, y: 40, scale: 0.9 }}
          transition={{ type: 'spring', stiffness: 300, damping: 22 }}
          role="alert"
          aria-live="polite"
        >
          <button className={styles.closeBtn} onClick={onClose} aria-label="Dismiss">×</button>
          <span className={styles.icon} aria-hidden="true">{achievement.icon}</span>
          <div className={styles.text}>
            <div className={styles.label}>Achievement Unlocked!</div>
            <div className={styles.title}>{achievement.title}</div>
            <div className={styles.desc}>{achievement.desc}</div>
          </div>
          <motion.div
            className={styles.progress}
            initial={{ width: '100%' }}
            animate={{ width: '0%' }}
            transition={{ duration: duration / 1000, ease: 'linear' }}
          />
        </motion.div>
      )}
    </AnimatePresence>
  );
};

/* Hook for triggering achievements */
export const useAchievements = () => {
  const [current, setCurrent] = useState<Achievement | null>(null);
  const [queue, setQueue] = useState<Achievement[]>([]);

  const unlock = (a: Achievement) => {
    setQueue(prev => [...prev, a]);
  };

  useEffect(() => {
    if (!current && queue.length > 0) {
      setCurrent(queue[0]);
      setQueue(prev => prev.slice(1));
    }
  }, [current, queue]);

  const dismiss = () => setCurrent(null);

  return { current, unlock, dismiss };
};

export const ACHIEVEMENTS: Record<string, Achievement> = {
  FIRST_LIKE:    { icon: '❤️', title: 'First Like',       desc: 'You liked your first review!' },
  FIRST_SAVE:    { icon: '🔖', title: 'Bookmarked!',      desc: 'Saved your first book to your list.' },
  JOINED_CLUB:   { icon: '🏛️', title: 'Club Member',      desc: 'You joined your first book club!' },
  WEEK_STREAK:   { icon: '🔥', title: '7-Day Streak',     desc: 'Reading every day for a week. You\'re on fire!' },
  LUCKY_READER:  { icon: '🎲', title: 'Feeling Lucky',    desc: 'You used Lucky Pick for the first time.' },
};
