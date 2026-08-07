import { useState } from 'react';
import styles from './PollCard.module.css';

interface Option {
  id: string;
  text: string;
  votes: number;
}

interface Props {
  question: string;
  initialOptions: Option[];
}

export const PollCard = ({ question, initialOptions }: Props) => {
  const [options, setOptions] = useState(initialOptions);
  const [votedId, setVotedId] = useState<string | null>(null);

  const totalVotes = options.reduce((sum, opt) => sum + opt.votes, 0);
  const maxVotes = Math.max(...options.map(o => o.votes));

  const handleVote = (id: string) => {
    if (votedId) return; // already voted
    setOptions(prev => prev.map(opt => opt.id === id ? { ...opt, votes: opt.votes + 1 } : opt));
    setVotedId(id);
  };

  return (
    <div className={styles.pollCard}>
      <div className={styles.pollHeader}>
        <div className={styles.pollIcon}>📊</div>
        <div>
          <div className={styles.pollTitle}>{question}</div>
          <div className={styles.pollSub}>{totalVotes + (votedId ? 1 : 0)} votes • 2 days left</div>
        </div>
      </div>

      <div className={styles.optionsList}>
        {options.map((opt) => {
          const pct = totalVotes === 0 ? 0 : Math.round((opt.votes / (totalVotes + (votedId ? 1 : 0))) * 100);
          const isWinner = votedId && opt.votes === (maxVotes + (opt.id === votedId ? 1 : 0));
          return (
            <div
              key={opt.id}
              className={`${styles.optionWrap} ${votedId ? styles.voted : ''} ${isWinner ? styles.winner : ''}`}
              onClick={() => handleVote(opt.id)}
            >
              {votedId && <div className={styles.optionFill} style={{ width: `${pct}%` }}></div>}
              <div className={styles.optionContent}>
                <span className={styles.optionText}>
                  {opt.text} {votedId && opt.id === votedId && '✓'}
                </span>
                {votedId && <span className={styles.optionPct}>{pct}%</span>}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};
