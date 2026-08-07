import { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import styles from './QuoteSharer.module.css';

interface Props {
  quote: string;
  bookTitle: string;
  author: string;
  onClose: () => void;
}

export const QuoteSharer = ({ quote, bookTitle, author, onClose }: Props) => {
  const [copied, setCopied] = useState(false);

  const handleCopy = async () => {
    const text = `"${quote}"\n— ${bookTitle} by ${author}\n\nvia BookShare`;
    await navigator.clipboard.writeText(text);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <AnimatePresence>
      <motion.div
        className={styles.overlay}
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        exit={{ opacity: 0 }}
        onClick={onClose}
      >
        <motion.div
          className={styles.card}
          initial={{ opacity: 0, y: 30, scale: 0.95 }}
          animate={{ opacity: 1, y: 0, scale: 1 }}
          exit={{ opacity: 0, y: 20, scale: 0.95 }}
          transition={{ type: 'spring', stiffness: 280, damping: 22 }}
          onClick={(e) => e.stopPropagation()}
        >
          <span className={styles.quoteMark} aria-hidden="true">"</span>
          <blockquote className={styles.quoteText}>"{quote}"</blockquote>
          <div className={styles.divider}></div>
          <div className={styles.bookInfo}>
            <div className={styles.bookTitle}>{bookTitle}</div>
            <div className={styles.bookAuthor}>— {author}</div>
          </div>
          <div className={styles.actions}>
            <button className={`${styles.btnCopy} ${copied ? styles.copied : ''}`} onClick={handleCopy}>
              {copied ? '✓ Copied!' : '📋 Copy Quote'}
            </button>
            <button className={styles.btnClose} onClick={onClose}>Close</button>
          </div>
        </motion.div>
      </motion.div>
    </AnimatePresence>
  );
};
