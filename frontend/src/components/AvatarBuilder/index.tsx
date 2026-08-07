import { useState } from 'react';
import styles from './AvatarBuilder.module.css';

const GRADIENTS = [
  'linear-gradient(135deg, #F59E0B, #EF4444)',
  'linear-gradient(135deg, #8B5CF6, #3B82F6)',
  'linear-gradient(135deg, #10B981, #3B82F6)',
  'linear-gradient(135deg, #EC4899, #8B5CF6)',
  'linear-gradient(135deg, #64748B, #0F172A)'
];

const EMOJIS = ['👽', '🦊', '👻', '🤖', '🦉', '🌵', '☕'];

export const AvatarBuilder = () => {
  const [bg, setBg] = useState(GRADIENTS[1]);
  const [emoji, setEmoji] = useState(EMOJIS[2]);

  return (
    <div className={styles.builderWrap}>
      <div className={styles.title}>Customize Avatar</div>
      
      <div className={styles.previewArea}>
        <div className={styles.avatar} style={{ background: bg }}>
          {emoji}
        </div>
      </div>

      <div className={styles.optionsSection}>
        <div className={styles.sectionLabel}>Background</div>
        <div className={styles.colorGrid}>
          {GRADIENTS.map(g => (
            <button
              key={g}
              className={`${styles.colorBtn} ${bg === g ? styles.active : ''}`}
              style={{ background: g }}
              onClick={() => setBg(g)}
              aria-label="Select background color"
            />
          ))}
        </div>
      </div>

      <div className={styles.optionsSection}>
        <div className={styles.sectionLabel}>Icon</div>
        <div className={styles.emojiGrid}>
          {EMOJIS.map(e => (
            <button
              key={e}
              className={`${styles.emojiBtn} ${emoji === e ? styles.active : ''}`}
              onClick={() => setEmoji(e)}
            >
              {e}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};
