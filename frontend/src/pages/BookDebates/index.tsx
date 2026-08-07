import { useState } from 'react';
import { Header } from '../../components/Header';
import { motion } from 'framer-motion';
import styles from './BookDebates.module.css';

const DEBATES = [
  {
    id: 1,
    book: 'Dune by Frank Herbert',
    topic: 'Is Paul Atreides a hero or a villain?',
    sideA: 'He is a tragic hero',
    sideB: 'He is a manipulative villain',
    votesA: 342,
    votesB: 215,
    argsA: [
      { name: 'Elena V.', initials: 'EV', color: '#7C3AED', text: 'He is trapped by his prescience. He chose the Golden Path because the alternative was extinction.' },
      { name: 'Liam S.', initials: 'LS', color: '#DC2626', text: 'He tries to stop the Jihad but the momentum of the myth he created is too strong.' }
    ],
    argsB: [
      { name: 'Asha R.', initials: 'AR', color: '#9A3412', text: 'He weaponized a religion to gain revenge for his father. Millions died for his vendetta.' }
    ]
  },
  {
    id: 2,
    book: 'The Creative Act by Rick Rubin',
    topic: 'Is creativity a mystical force or a trainable muscle?',
    sideA: 'Mystical force (The Source)',
    sideB: 'Trainable muscle (Discipline)',
    votesA: 189,
    votesB: 204,
    argsA: [
      { name: 'Nora K.', initials: 'NK', color: '#4F46E5', text: 'Rubin makes it clear we are vessels. The ideas exist out there, we just have to be open to receive them.' }
    ],
    argsB: [
      { name: 'Mark J.', initials: 'MJ', color: '#059669', text: 'It\'s about showing up every day. The "mystical" part only happens when you put in the hours of practice.' },
      { name: 'Jane D.', initials: 'JD', color: '#F59E0B', text: 'If it was just mystical, you couldn\'t improve. But you can. It\'s a skill.' }
    ]
  }
];

export default function BookDebates() {
  const [debates, setDebates] = useState(DEBATES);
  const [voted, setVoted] = useState<Record<number, 'A' | 'B'>>({});

  const handleVote = (debateId: number, side: 'A' | 'B') => {
    if (voted[debateId]) return; // Already voted

    setDebates(prev => prev.map(d => {
      if (d.id === debateId) {
        return {
          ...d,
          votesA: side === 'A' ? d.votesA + 1 : d.votesA,
          votesB: side === 'B' ? d.votesB + 1 : d.votesB
        };
      }
      return d;
    }));
    setVoted(prev => ({ ...prev, [debateId]: side }));
  };

  return (
    <div className={styles.page}>
      <Header />
      
      <div className={styles.hero}>
        <div className={styles.kicker}><span></span> Community Debates</div>
        <h1 className={styles.heroTitle}>Pick a Side</h1>
        <p className={styles.heroSub}>Dive into the most polarizing questions in literature. Read the arguments, cast your vote, and shift the balance.</p>
      </div>

      <div className={styles.content}>
        {debates.map(debate => {
          const total = debate.votesA + debate.votesB;
          const pctA = Math.round((debate.votesA / total) * 100);
          const pctB = 100 - pctA;
          const hasVoted = !!voted[debate.id];

          return (
            <motion.div key={debate.id} className={styles.debateCard} initial={{ opacity: 0, y: 20 }} whileInView={{ opacity: 1, y: 0 }} viewport={{ once: true }}>
              <div className={styles.debateHeader}>
                <div className={styles.debateBook}>📖 {debate.book}</div>
                <div className={styles.debateTopic}>{debate.topic}</div>
              </div>

              <div className={styles.battleZone}>
                <div className={styles.meterWrap}>
                  <div className={styles.meterRow}>
                    <span className={styles.sideA}>{pctA}%</span>
                    <span className={styles.sideB}>{pctB}%</span>
                  </div>
                  <div className={styles.meter}>
                    <div className={styles.meterFillA} style={{ width: `${pctA}%` }}></div>
                    <div className={styles.meterFillB} style={{ width: `${pctB}%` }}></div>
                  </div>
                  <div className={styles.meterRow} style={{ font: '500 12px/1 var(--font-body)', color: 'var(--muted)', marginTop: 4 }}>
                    <span>{debate.sideA}</span>
                    <span>{debate.sideB}</span>
                  </div>
                </div>

                <div className={styles.sidesGrid}>
                  <div className={styles.sideCol}>
                    {debate.argsA.map((arg, i) => (
                      <div key={i} className={`${styles.argCard} ${styles.argCardA}`}>
                        <div className={styles.argUser}>
                          <div className={styles.argAvatar} style={{ background: arg.color }}>{arg.initials}</div>
                          <div className={styles.argName}>{arg.name}</div>
                        </div>
                        <div className={styles.argText}>{arg.text}</div>
                      </div>
                    ))}
                    {!hasVoted && (
                      <button className={`${styles.btnVote} ${styles.btnVoteA}`} onClick={() => handleVote(debate.id, 'A')}>
                        Vote: {debate.sideA}
                      </button>
                    )}
                  </div>

                  <div className={styles.sideCol}>
                    {debate.argsB.map((arg, i) => (
                      <div key={i} className={`${styles.argCard} ${styles.argCardB}`}>
                        <div className={styles.argUser}>
                          <div className={styles.argAvatar} style={{ background: arg.color }}>{arg.initials}</div>
                          <div className={styles.argName}>{arg.name}</div>
                        </div>
                        <div className={styles.argText}>{arg.text}</div>
                      </div>
                    ))}
                    {!hasVoted && (
                      <button className={`${styles.btnVote} ${styles.btnVoteB}`} onClick={() => handleVote(debate.id, 'B')}>
                        Vote: {debate.sideB}
                      </button>
                    )}
                  </div>
                </div>
              </div>
            </motion.div>
          );
        })}
      </div>
    </div>
  );
}
